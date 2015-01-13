using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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

        public static readonly DependencyProperty timeInProperty = DependencyProperty.Register("timeIn", typeof(DateTime), typeof(TimeEntry),
                new FrameworkPropertyMetadata(DateTime.Now));
        public static readonly DependencyProperty timeOutProperty = DependencyProperty.Register("timeOut", typeof(DateTime), typeof(TimeEntry),
                new FrameworkPropertyMetadata(DateTime.Now));
        public static readonly DependencyProperty timeSpentProperty = DependencyProperty.Register("timeSpent", typeof(TimeSpan), typeof(TimeEntry),
                new FrameworkPropertyMetadata(TimeSpan.Zero));
        public static readonly DependencyProperty commentProperty = DependencyProperty.Register("comment", typeof(string), typeof(TimeEntry),
                new FrameworkPropertyMetadata(defaultComment));
        public static readonly DependencyPropertyKey subCommentsPropertyKey = DependencyProperty.RegisterReadOnly("subComments", typeof(List<string>),
            typeof(TimeEntry),
                new FrameworkPropertyMetadata(new List<string>()));
        public static readonly DependencyProperty subCommentsProperty = subCommentsPropertyKey.DependencyProperty;

        public static readonly DependencyProperty controlWidthProperty = DependencyProperty.Register("controlWidth", typeof(double), typeof(TimeEntry), new FrameworkPropertyMetadata(0.0));

        public static readonly DependencyProperty controlHeightProperty = DependencyProperty.Register("controlHeight", typeof(double), typeof(TimeEntry), new FrameworkPropertyMetadata(0.0));

        public static readonly DependencyProperty detailsExpandedProperty =
            DependencyProperty.Register("detailsExpanded", typeof(bool), typeof(TimeEntry),
            new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnDetailsExpanded)));

        public static readonly DependencyProperty alarmExpandedProperty =
            DependencyProperty.Register("alarmExpanded", typeof(bool),
            typeof(TimeEntry), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnAlarmExpandedChanged)));

        public static readonly RoutedEvent deleteEvent = EventManager.RegisterRoutedEvent("delete", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(TimeEntry));

        private TransformGroup animatedTransform;
        private ScaleTransform animatedScale;

        private DoubleAnimation deleteDoubleAnimation;

        #region Properties
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
        #endregion

        static TimeEntry() { }

        public TimeEntry()
        {
            timeIn = DateTime.Now;
            timeSpent = timeOut - timeIn;
            this.InitializeComponent();
            setUpRenderTransform();
            setUpAnimationVariables();
            deleteDoubleAnimation.Completed += deleteDoubleAnimation_Completed;
        }

        private void setUpRenderTransform()
        {
            animatedTransform = new TransformGroup();
            animatedScale = new ScaleTransform();
            animatedTransform.Children.Add(animatedScale);

            this.RenderTransform = animatedTransform;
        }

        private void setUpAnimationVariables()
        {
            deleteDoubleAnimation = new DoubleAnimation();
            deleteDoubleAnimation.To = 0;
            deleteDoubleAnimation.Duration = new Duration(TimeSpan.Parse("0:0:.25"));
        }

        private void commentBox_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            comment = (sender as TextBox).Text;
        }

        #region delete event methods
        private void raiseDeleteEvent()
        {
            RoutedEventArgs newDeleteEvent = new RoutedEventArgs(deleteEvent, this);
            RaiseEvent(newDeleteEvent);
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            deleteAnimation();
        }

        void deleteDoubleAnimation_Completed(object sender, EventArgs e)
        {
            raiseDeleteEvent();
        }

        private void deleteAnimation()
        {
            animatedScale.BeginAnimation(ScaleTransform.ScaleXProperty, deleteDoubleAnimation);
            animatedScale.BeginAnimation(ScaleTransform.ScaleYProperty, deleteDoubleAnimation);
            this.BeginAnimation(FrameworkElement.HeightProperty, deleteDoubleAnimation);
        }
        #endregion

        private void commentConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            comment = commentBox.Text;
        }

        private static void OnDetailsExpanded(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimeEntry sentinel = d as TimeEntry;

            if (sentinel.detailsExpanded)
            {
                sentinel.alarmExpanded = false;
            }
        }

        private static void OnAlarmExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimeEntry sentinel = d as TimeEntry;

            if (sentinel.alarmExpanded)
            {
                sentinel.detailsExpanded = false;
            }

        }

        public bool commentDefault()
        {
            if (comment == defaultComment)
                return true;
            return false;
        }

        private void detailsButton_Click(object sender, RoutedEventArgs e)
        {
            detailsExpanded = !detailsExpanded;
        }
    }
}