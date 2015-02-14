using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace ClockWatcher
{
    [Serializable]
    public class SessionManager : DependencyObject
    {
        public static readonly DependencyProperty currentSessionProperty =
            DependencyProperty.Register("currentSession", typeof(Session),
            typeof(MainWindow), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty startTimeProperty =
            DependencyProperty.Register("strStartTime", typeof(string),
            typeof(MainWindow), new FrameworkPropertyMetadata(DateTime.Now.ToString()));


        public delegate void newAddedCommentEventHandler(commentEntry entry);
        public delegate void timeEntryDeletedEventHandler(int id);
        public delegate void commentEntryDeletedEventHandler(int id);

        public event newAddedCommentEventHandler newAddedCommentEvent;
        public event timeEntryDeletedEventHandler timeEntryDeletedEvent;
        public event commentEntryDeletedEventHandler commentEntryDeletedEvent;
        /// <summary>
        /// Holds a list of all comments made within the session
        /// </summary>
        private List<Session> Sessions;
        private bool _isWatching;
        private Timer _timer;
        private Stopwatch _clockWatch;
        private bool isWatching
        {
            get
            {
                return _isWatching;
            }
            set
            {
                _isWatching = value;
                if (!_isWatching)
                {
                    _clockWatch.Stop();
                    _timer.Stop();
                }
                else
                {
                    _clockWatch.Start();
                    _timer.Start();
                }
            }
        }
        private DateTime _dtStartTime;
        #region Properties

        public List<commentEntry> commentLibrary { get; private set; }
        public Session currentSession
        {
            get
            {
                return (Session)GetValue(currentSessionProperty);
            }
            set
            {
                SetValue(currentSessionProperty, value);
            }
        }
        public string strStartTime
        {
            get
            {
                return (string)GetValue(startTimeProperty);
            }
            set
            {
                SetValue(startTimeProperty, value);
            }
        }
        #endregion

        public SessionManager()
        {
            _clockWatch = new Stopwatch();
            _timer = new Timer(1000);//one second
            _timer.Elapsed += clockWatch_Elapsed;

            commentLibrary = new List<commentEntry>();

            _dtStartTime = DateTime.Now;

            Sessions = new List<Session>();

            Sessions.Add(new Session());
            currentSession = Sessions[0];
            _isWatching = false;
        }
        #region Methods
        #region Event Methods
        /// <summary>
        /// Registered to Tier.Elapsed Event
        /// (See constructor)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void clockWatch_Elapsed(object sender, ElapsedEventArgs e)
        {
            /*
            Dispatcher.Invoke((Action)(() =>
                {
                    updateTimeSpent();
                }));
            */
            Dispatcher.Invoke(updateTimeSpent);
        }
        /// <summary>
        /// Registered to TimeEntry.newCommentEvent Event
        /// (see addNewTimeEntry())
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void newCommentEntry(string comment)
        {
            foreach (commentEntry ce in commentLibrary)
            {
                if (ce.comment == comment)
                    return;
            }

            //If you made it this far, then none of them matched
            commentEntry newEntry = new commentEntry(commentLibrary.Count, comment);
            newEntry.delete += commentEntry_delete;
            newEntry.checkedEvent += newEntry_checkedEvent;
            commentLibrary.Add(newEntry);
            if (newAddedCommentEvent != null)
                newAddedCommentEvent(newEntry);
        }
        /// <summary>
        /// Registered to TimeEntry.delete Event
        /// (see addNewTimeEntry())
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void timeEntry_delete(object sender)
        {
            TimeEntry sentinel = sender as TimeEntry;
            if (currentSession.timeEntries.Count > 0 && sentinel.entryID > currentSession.timeEntries.Last().entryID)
            {
                isWatching = false;
                _clockWatch.Reset();
            }

            if (timeEntryDeletedEvent != null)
                timeEntryDeletedEvent(sentinel.entryID);

            calculateTotalTime();
        }
        /// <summary>
        /// Registered commentEntry.delete Event
        /// (see newCommentEntry())
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void commentEntry_delete(object sender, RoutedEventArgs e)
        {
            commentEntry sentinel = sender as commentEntry;
            /*
             * Remove the comment from all TimeEntries whose
             * comment matches this one.
            */
            foreach (TimeEntry te in currentSession.timeEntries)
            {
                if (sentinel.comment == te.comment)
                {
                    te.comment = "";
                }
            }
            commentLibrary.Remove(sentinel);
            if (commentEntryDeletedEvent != null)
                commentEntryDeletedEvent(sentinel.entryID);
        }
        /// <summary>
        /// Registered to commentEntry.checkedEvent Event
        /// (see newCommentEntry())
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="rea"></param>
        public void newEntry_checkedEvent(object sender, RoutedEventArgs rea)
        {
            commentEntry sentinel = sender as commentEntry;
            bool isOneChecked = sentinel.isChecked;
            //first check to see if there are other comments checked
            if (!sentinel.isChecked)
            {
                foreach (commentEntry ce in commentLibrary)
                {
                    if (ce != sentinel && ce.isChecked)
                    {
                        isOneChecked = true;
                        break;
                    }
                }
            }
            if (isOneChecked)
            {
                //Do basic filtering

                foreach (TimeEntry te in currentSession.timeEntries)
                {
                    /*
                     * If the comments don't match and this entry wasn't
                     * tagged by another checked commentEntry to stay visibile...
                    */
                    if (te.comment != sentinel.comment && !te.isMarkedForView)
                    {
                        //...then close it.
                        te.isCollapsed = sentinel.isChecked;
                    }
                    else
                    {
                        //if the comments match...
                        if (te.comment == sentinel.comment)
                        {
                            /*
                             * ...then mark it to stay open, so no other
                             * commentEntries close it by mistake
                            */
                            te.isCollapsed = !sentinel.isChecked;
                            te.isMarkedForView = true;
                        }
                    }
                }
            }
            else
            {
                //set all timeEntries' collapsed variables to false
                foreach (TimeEntry te in currentSession.timeEntries)
                {
                    te.isCollapsed = false;
                    te.isMarkedForView = false;
                }
            }
        }
        #endregion
        #region Class Methods
        public void addNewTimeEntry()
        {
            if (currentSession != null && currentSession.currentTimeEntry != null)
            {
                isWatching = false;
                currentSession.currentTimeEntry.finalizeTimeEntry();
            }
            currentSession.addEntry(DateTime.Now);
            _clockWatch.Reset();
            isWatching = true;
            currentSession.currentTimeEntry.deleteEvent += timeEntry_delete;
            currentSession.currentTimeEntry.newCommentEvent += newCommentEntry;
            //return currentSession.currentTimeEntry;
        }
        public void calculateTotalTime()
        {
            currentSession.totalTime = TimeSpan.Zero;
            foreach (TimeEntry te in currentSession.timeEntries)
            {
                currentSession.totalTime += te.timeSpent;
            }
        }
        private void updateTimeSpent()
        {
            if (currentSession != null)
            {
                //update the timespent variable of the current timeEntry
                if (currentSession.currentTimeEntry != null)
                {
                    currentSession.currentTimeEntry.timeSpent = _clockWatch.Elapsed;
                    calculateTotalTime();
                }
            }
        }
        #endregion
        #endregion
    }
}
