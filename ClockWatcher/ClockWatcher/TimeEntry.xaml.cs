using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClockWatcher
{
    [Serializable]
    /// <summary>
    /// Interaction logic for timeEntry.xaml
    /// </summary>
    public partial class TimeEntry : UserControl
    {
        private static string defaultComment = "Type Comment Here";

        #region Dependency Properties
        public static readonly DependencyProperty alarmExpandedProperty =
            DependencyProperty.Register("alarmExpanded", typeof(bool),
            typeof(TimeEntry), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnAlarmExpandedChanged)));
        public static readonly DependencyProperty controlHeightProperty = DependencyProperty.Register("controlHeight", typeof(double),
            typeof(TimeEntry), new FrameworkPropertyMetadata(0.0));
        public static readonly DependencyProperty controlWidthProperty = DependencyProperty.Register("controlWidth", typeof(double),
            typeof(TimeEntry), new FrameworkPropertyMetadata(0.0));
        public static readonly DependencyProperty commentProperty = DependencyProperty.Register("comment", typeof(string), typeof(TimeEntry),
                new FrameworkPropertyMetadata(defaultComment));
        public static readonly DependencyProperty detailsExpandedProperty =
            DependencyProperty.Register("detailsExpanded", typeof(bool), typeof(TimeEntry),
            new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnDetailsExpanded)));
        public static readonly DependencyProperty timeInProperty = DependencyProperty.Register("timeIn", typeof(DateTime), typeof(TimeEntry),
                new FrameworkPropertyMetadata(DateTime.Now));
        public static readonly DependencyProperty timeOutProperty = DependencyProperty.Register("timeOut", typeof(DateTime), typeof(TimeEntry),
                new FrameworkPropertyMetadata(DateTime.Now));
        public static readonly DependencyProperty timeSpentProperty = DependencyProperty.Register("timeSpent", typeof(TimeSpan), typeof(TimeEntry),
                new FrameworkPropertyMetadata(TimeSpan.Zero));
        public static readonly DependencyProperty selectedProperty = DependencyProperty.Register("isSelected", typeof(bool),
            typeof(TimeEntry), new PropertyMetadata(false));

        public static readonly DependencyPropertyKey subCommentsPropertyKey = DependencyProperty.RegisterReadOnly("subComments", typeof(List<string>),
            typeof(TimeEntry),
                new FrameworkPropertyMetadata(new List<string>()));
        public static readonly DependencyProperty subCommentsProperty = subCommentsPropertyKey.DependencyProperty;
        #endregion

        #region Routed Events
        public static readonly RoutedEvent deleteEvent = EventManager.RegisterRoutedEvent("delete", RoutingStrategy.Direct,
            typeof(RoutedEventHandler), typeof(TimeEntry));
        #endregion

        private TransformGroup animatedTransform;
        private ScaleTransform animatedScale;

        private DoubleAnimation deleteDoubleAnimation;

        public delegate void textChangedEventHandler(object sender, TextChangedEventArgs tcea);
        public delegate void enterPressEventHandler(object sender, KeyEventArgs kea);
        public delegate void newCommentEventHandler(string comment);

        public event enterPressEventHandler enterPressEvent;
        public event textChangedEventHandler textChangedEvent;
        public event newCommentEventHandler newCommentEvent;

        /// <summary>
        /// This will be used along with isCollapsed to ensure
        /// this entry was marked as collapsed/uncollapsed by
        /// a commentEntry being checked.
        /// </summary>
        private bool _isMarkedForView,_isCollapsed;
        private int _entryID;
        #region Properties
        #region Dependency Properties
        public bool alarmExpanded
        {
            get
            {
                return (bool)GetValue(alarmExpandedProperty);
            }
            set
            {
                SetValue(alarmExpandedProperty, value);
            }
        }
        public double controlHeight
        {
            get
            {
                return (double)GetValue(controlHeightProperty);
            }
            set
            {
                SetValue(controlHeightProperty, value);
            }
        }
        public double controlWidth
        {
            get
            {
                return (double)GetValue(controlWidthProperty);
            }
            set
            {
                SetValue(controlWidthProperty, value);
            }
        }
        public string comment
        {
            get
            {
                return (string)GetValue(TimeEntry.commentProperty);
            }
            set
            {
                SetValue(TimeEntry.commentProperty, value);
            }
        }
        public bool detailsExpanded
        {
            get
            {
                return (bool)GetValue(detailsExpandedProperty);
            }
            set
            {
                SetValue(detailsExpandedProperty, value);
            }
        }
        public int entryID
        {
            get
            {
                return _entryID;
            }
            private set
            {
                _entryID = value;
            }
        }
        public bool isCollapsed
        {
            get
            {
                return _isCollapsed;
            }
            set
            {
                _isCollapsed = value;
            }
        }
        public bool isMarkedForView
        {
            get
            {
                return _isMarkedForView;
            }
            set
            {
                _isMarkedForView = value;
            }
        }
        public bool isSelected
        {
            get
            {
                return (bool)GetValue(selectedProperty);
            }
            set
            {
                SetValue(selectedProperty, value);
            }
        }
        public List<string> subComments
        {
            get
            {
                return (List<string>)GetValue(TimeEntry.subCommentsProperty);
            }
            set
            {
                SetValue(TimeEntry.subCommentsProperty, value);
            }
        }
        public DateTime timeIn
        {
            get
            {
                return (DateTime)GetValue(TimeEntry.timeInProperty);
            }
            set
            {
                SetValue(TimeEntry.timeInProperty, value);
            }
        }
        public DateTime timeOut
        {
            get
            {
                return (DateTime)GetValue(TimeEntry.timeOutProperty);
            }
            set
            {
                SetValue(TimeEntry.timeOutProperty, value);
            }
        }
        public TimeSpan timeSpent
        {
            get
            {
                return (TimeSpan)GetValue(TimeEntry.timeSpentProperty);
            }
            set
            {
                SetValue(TimeEntry.timeSpentProperty, value);
            }
        }

        #endregion

        #region Routed Event Properties
        public event RoutedEventHandler delete
        {
            add
            {
                AddHandler(deleteEvent, value);
            }
            remove
            {
                RemoveHandler(deleteEvent, value);
            }
        }
        #endregion
        #endregion

        static TimeEntry() { }

        public TimeEntry(int id)
        {
            setUpRenderTransform();
            setUpAnimationVariables();
            DataContext = this;
            this.InitializeComponent();
            deleteDoubleAnimation.Completed += deleteDoubleAnimation_Completed;
            entryID = id;
        }

        #region Methods
        #region Event Methods
        private void commentBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SolidColorBrush foregroundBrush = new SolidColorBrush((Color)Resources["blackCommentColor"]);
            commentBox.Foreground = foregroundBrush;
            if (commentBox.Text == defaultComment)
                commentBox.Text = "";
        }
        private void commentBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SolidColorBrush foregroundBrush = new SolidColorBrush((Color)Resources["grayCommentColor"]);
            commentBox.Foreground = foregroundBrush;
        }
        private void commentBox_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            comment = (sender as TextBox).Text;
        }
        private void commentBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            comment = (sender as TextBox).Text;
            if (textChangedEvent != null)
            {
                textChangedEvent(sender, e);
            }
        }
        private void commentConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (newCommentEvent != null)
                newCommentEvent(comment);
        }
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            deleteAnimation();
        }
        private void deleteDoubleAnimation_Completed(object sender, EventArgs e)
        {
            RoutedEventArgs newDeleteEvent = new RoutedEventArgs(deleteEvent, this);
            RaiseEvent(newDeleteEvent);
        }
        private void detailsButton_Click(object sender, RoutedEventArgs e)
        {
            detailsExpanded = !detailsExpanded;
        }
        private static void OnAlarmExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimeEntry sentinel = d as TimeEntry;

            if (sentinel.alarmExpanded)
            {
                sentinel.detailsExpanded = false;
            }
        }
        private static void OnDetailsExpanded(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimeEntry sentinel = d as TimeEntry;

            if (sentinel.detailsExpanded)
            {
                sentinel.alarmExpanded = false;
            }
        }
        private void UserControl_KeyDown(object sender, KeyEventArgs kea)
        {
            if (kea.Key == Key.Enter)
            {
                if (commentBox.IsFocused)
                {
                    if (commentBox.Text != defaultComment && commentBox.Text != "")
                    {
                        this.Focus();
                        if (newCommentEvent != null)
                            newCommentEvent(commentBox.Text);
                    }
                }
            }
        }
        #endregion

        #region Class Methods
        /// <summary>
        /// Clears the comment's context
        /// </summary>
        public void ClearComment()
        {
            comment = string.Empty;
        }
        /// <summary>
        /// Determines whether or not the TimeEntry's comment contains the default text
        /// </summary>
        /// <returns>returns true if it does, else it returns false</returns>
        public bool commentDefault()
        {
            if (comment == defaultComment)
                return true;
            return false;
        }
        private void deleteAnimation()
        {
            animatedScale.BeginAnimation(ScaleTransform.ScaleXProperty, deleteDoubleAnimation);
            animatedScale.BeginAnimation(ScaleTransform.ScaleYProperty, deleteDoubleAnimation);
            this.BeginAnimation(FrameworkElement.HeightProperty, deleteDoubleAnimation);
        }
        /// <summary>
        /// Adds the timeOut data for the TimeEntry
        /// </summary>
        public void finalizeTimeEntry()
        {
            timeOut = DateTime.Now;
            timeOutBlock.Text = timeOut.TimeOfDay.ToString();
        }
        private void setUpAnimationVariables()
        {
            deleteDoubleAnimation = new DoubleAnimation();
            deleteDoubleAnimation.To = 0;
            deleteDoubleAnimation.Duration = new Duration(TimeSpan.Parse("0:0:.25"));
        }
        private void setUpRenderTransform()
        {
            animatedTransform = new TransformGroup();
            animatedScale = new ScaleTransform();
            animatedTransform.Children.Add(animatedScale);

            this.RenderTransform = animatedTransform;
        }
        #endregion

        #endregion



    }
}