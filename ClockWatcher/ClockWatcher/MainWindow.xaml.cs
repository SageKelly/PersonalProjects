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

        Stopwatch timer;
        Timer clockWatch;
        List<commentEntry> commentLibrary;
        public string defaultComment { get; private set; }

        private bool _isWatching, _filterSelected;

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
            commentLibrary = new List<commentEntry>();
            defaultComment = "Add New Comment";
            commentAddingBox.Text = defaultComment;
            _filterSelected = false;
            _isWatching = false;
            dtStartTime = DateTime.Now;
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

        private void calculateTotalTime()
        {
            currentSession.totalTime = TimeSpan.Zero;
            foreach (TimeEntry te in currentSession.timeEntries)
            {
                currentSession.totalTime += te.timeSpent;
            }
        }

        private void entryAdder_Click(object sender, RoutedEventArgs e)
        {
            addNewTime();
        }

        private void addNewTime()
        {
            if (currentSession != null && currentSession.currentTimeEntry != null)
            {
                isWatching = false;
            }
            currentSession.addEntry(DateTime.Now);
            scrollStack.Children.Add(currentSession.currentTimeEntry);
            //Subscribe to the deletion event
            currentSession.currentTimeEntry.delete += currentTimeEntry_delete;
            isWatching = true;
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

        private void commentAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (commentAddingBox.Text != defaultComment && commentAddingBox.Text != "")
            {
                addNewComment(commentAddingBox.Text);
            }
        }

        private void addNewComment(string comment)
        {
            commentEntry newEntry = new commentEntry(commentAddingBox.Text);
            newEntry.delete += entry_delete;
            commentLibrary.Add(newEntry);
            commentStack.Children.Add(newEntry);
        }

        void entry_delete(object sender, RoutedEventArgs e)
        {
            commentLibrary.Remove((sender as commentEntry));
            commentStack.Children.Remove((sender as commentEntry));
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
                case Key.OemEnlw:
                    if (commentAddingBox.IsFocused)
                        if (commentAddingBox.Text != defaultComment && commentAddingBox.Text != "")
                            addNewComment(commentAddingBox.Text);
                        else if (currentSession.currentTimeEntry != null && currentSession.currentTimeEntry.commentBox.IsFocused)
                        {
                            if (currentSession.currentTimeEntry.commentDefault())
                            {
                                currentSession.currentTimeEntry.comment = currentSession.currentTimeEntry.commentBox.Text;
                            }
                        }
                    break;
                case Key.Space:
                    addNewTime();
                    break;
            }
        }

        private void commentAddingBox_LostFocus(object sender, RoutedEventArgs e)
        {
            commentAddingBox.Text = defaultComment;
        }

        private void filterTimeEntriesByComment()
        {
            throw new NotImplementedException();
        }

    }
}