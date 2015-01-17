using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls.Primitives;

namespace ClockWatcher
{
    [Serializable]
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty startTimeProperty =
            DependencyProperty.Register("strStartTime", typeof(string),
            typeof(MainWindow), new FrameworkPropertyMetadata(DateTime.Now.ToString()));

        public static readonly DependencyProperty currentSessionProperty =
            DependencyProperty.Register("currentSession", typeof(Session),
            typeof(MainWindow), new FrameworkPropertyMetadata(null));

        List<Session> Sessions;

        Stopwatch timer;
        Timer clockWatch;

        /// <summary>
        /// Holds a list of all comments made within the session
        /// </summary>
        ObservableCollection<commentEntry> commentLibrary;
        /// <summary>
        /// Represents, during selection mode, which TimeEntry is currently selected.
        /// </summary>
        TimeEntry currentSelectedEntry;

        int selectionIndex;
        public string defaultComment { get; private set; }

        private bool _isWatching, _filterSelected, _isSelecting;

        #region Properties
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
        public bool isWatching
        {
            get
            {
                return _isWatching;
            }
            private set
            {
                _isWatching = value;
                if (!_isWatching)
                {
                    timer.Stop();
                    clockWatch.Stop();
                }
                else
                {
                    timer.Start();
                    clockWatch.Start();
                }
            }
        }
        private bool filterSelected
        {
            get
            {
                return _filterSelected;
            }
            set
            {
                _filterSelected = value;
                if (_filterSelected)
                {
                    filterTimeEntriesByComment();
                }
            }
        }
        public string strStartTime
        {
            get
            {
                return (string)GetValue(MainWindow.startTimeProperty);
            }
            set
            {
                SetValue(MainWindow.startTimeProperty, value);
            }
        }
        public DateTime dtStartTime { get; private set; }
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            Sessions = new List<Session>();
            Sessions.Add(new Session());
            currentSession = Sessions[0];
            timer = new Stopwatch();
            clockWatch = new Timer(1000);//one minute
            clockWatch.Elapsed += clockWatch_Elapsed;
            DataContext = currentSession;
            commentLibrary = new ObservableCollection<commentEntry>();
            defaultComment = "Add New Comment";
            commentAddingBox.Text = defaultComment;
            _filterSelected = false;
            _isWatching = false;
            _isSelecting = false;
            dtStartTime = DateTime.Now;
            SetUpListBoxBinding();
        }

        #region Methods
        #region Event Methods
        private void commentAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (commentAddingBox.Text != defaultComment && commentAddingBox.Text != "")
            {
                addNewComment(commentAddingBox.Text);
            }
        }
        void currentTimeEntry_delete(object sender, RoutedEventArgs e)
        {
            if (scrollStack.Children.Count > 0 &&
                (sender as TimeEntry).Equals(
                scrollStack.Children[scrollStack.Children.Count - 1]))
            {
                isWatching = false;
                timer.Reset();
            }
            scrollStack.Children.Remove(sender as TimeEntry);
            calculateTotalTime();
        }
        void currentTimeEntry_newComment(object sender, RoutedEventArgs e)
        {
            TimeEntry sentinel = sender as TimeEntry;
            foreach (commentEntry ce in commentLibrary)
            {
                if (ce.comment == sentinel.comment)
                    return;
            }
            //If you made it this far, then none of them matched
            addNewComment(sentinel.comment);
        }
        void clockWatch_Elapsed(object sender, ElapsedEventArgs e)
        {
            /*
            Dispatcher.Invoke((Action)(() =>
                {
                    updateTimeSpent();
                }));
            */
            Dispatcher.Invoke(updateTimeSpent);
        }
        private void entryAdder_Click(object sender, RoutedEventArgs e)
        {
            addNewTime();
        }
        private void commentAddingBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).Text = "";
        }
        private void KeysDown(object sender, KeyEventArgs kea)
        {
            switch (kea.Key)
            {
                case Key.Enter:
                    if (commentAddingBox.IsFocused)
                    {
                        if (commentAddingBox.Text != defaultComment && commentAddingBox.Text != "")
                            addNewComment(commentAddingBox.Text);
                    }
                    if (_isSelecting)
                    {

                    }
                    break;
                case Key.Space:
                    if (!_isSelecting)
                    {
                        addNewTime();
                    }
                    break;
                /*
            case Key.E:
                    
                if (!_isSelecting && currentSession.timeEntries.Count != 0)
                {
                    selectionIndex = 0;
                    currentSelectedEntry = currentSession.timeEntries[selectionIndex];
                    currentSelectedEntry.isSelected = true;
                    _isSelecting = true;
                }
                else if (_isSelecting)
                {
                    currentSelectedEntry.isSelected = false;
                    _isSelecting = false;
                }
                break;
                */
                case Key.Up:
                    if (_isSelecting)
                    {
                        if (selectionIndex == 0)
                            selectionIndex = currentSession.timeEntries.Count - 1;
                        else
                            selectionIndex--;
                        currentSelectedEntry.isSelected = false;
                        currentSelectedEntry = currentSession.timeEntries[selectionIndex];
                        currentSelectedEntry.isSelected = true;
                        //currentSelectedEntry.Focus();//this works for forcing scroll
                    }
                    break;
                case Key.Down:
                    if (_isSelecting)
                    {
                        selectionIndex = (selectionIndex + 1) % currentSession.timeEntries.Count;
                        currentSelectedEntry.isSelected = false;
                        currentSelectedEntry = currentSession.timeEntries[selectionIndex];
                        currentSelectedEntry.isSelected = true;
                    }
                    break;
            }
        }
        private void commentAddingBox_LostFocus(object sender, RoutedEventArgs e)
        {
            commentAddingBox.Text = defaultComment;
        }
        void entry_delete(object sender, RoutedEventArgs e)
        {
            commentLibrary.Remove((sender as commentEntry));
            commentStack.Children.Remove((sender as commentEntry));
        }
        #endregion

        #region Class Methods
        private void addNewComment(string comment)
        {
            commentEntry newEntry = new commentEntry(comment);
            newEntry.delete += entry_delete;
            commentLibrary.Add(newEntry);
            commentStack.Children.Add(newEntry);
        }
        private void addNewTime()
        {
            if (currentSession != null && currentSession.currentTimeEntry != null)
            {
                isWatching = false;
                currentSession.currentTimeEntry.finalizeTimeEntry();
            }
            currentSession.addEntry(DateTime.Now, commentLibrary);
            scrollStack.Children.Add(currentSession.currentTimeEntry);
            //Subscribe to the assorted events
            currentSession.currentTimeEntry.delete += currentTimeEntry_delete;
            currentSession.currentTimeEntry.newComment += currentTimeEntry_newComment;
            timer.Reset();
            isWatching = true;
        }

        private void calculateTotalTime()
        {
            currentSession.totalTime = TimeSpan.Zero;
            foreach (TimeEntry te in currentSession.timeEntries)
            {
                currentSession.totalTime += te.timeSpent;
            }
        }
        private void filterTimeEntriesByComment()
        {
            throw new NotImplementedException();
        }
        private void SetUpListBoxBinding()
        {
        }
        private void updateTimeSpent()
        {
            if (currentSession != null)
            {
                //update the timespent variable of the current timeEntry
                if (currentSession.currentTimeEntry != null)
                {
                    currentSession.currentTimeEntry.timeSpent = timer.Elapsed;
                    calculateTotalTime();
                }
            }
        }
        #endregion

        #endregion
    }
}