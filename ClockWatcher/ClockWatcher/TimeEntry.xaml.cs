﻿using System;
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
        public static readonly DependencyProperty controlHeightProperty = DependencyProperty.Register("controlHeight", typeof(double), typeof(TimeEntry), new FrameworkPropertyMetadata(0.0));
        public static readonly DependencyProperty controlWidthProperty = DependencyProperty.Register("controlWidth", typeof(double), typeof(TimeEntry), new FrameworkPropertyMetadata(0.0));
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
        public static readonly RoutedEvent deleteEvent = EventManager.RegisterRoutedEvent("delete", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(TimeEntry));
        public static readonly RoutedEvent newCommentEvent = EventManager.RegisterRoutedEvent("newComment", RoutingStrategy.Direct,
            typeof(RoutedEventHandler), typeof(TimeEntry));
        #endregion

        private TransformGroup animatedTransform;
        private ScaleTransform animatedScale;

        private DoubleAnimation deleteDoubleAnimation;

        private ObservableCollection<commentEntry> commentLibrary;

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
        public event RoutedEventHandler newComment
        {
            add
            {
                AddHandler(newCommentEvent, value);
            }
            remove
            {
                RemoveHandler(newCommentEvent, value);
            }
        }
        #endregion
        #endregion

        static TimeEntry() { }

        private TimeEntry()
        {
            setUpRenderTransform();
            setUpAnimationVariables();
            commentLibrary = new ObservableCollection<commentEntry>();
            DataContext = this;
            this.InitializeComponent();
            deleteDoubleAnimation.Completed += deleteDoubleAnimation_Completed;
        }

        public TimeEntry(ObservableCollection<commentEntry> commentList)
            : this()
        {
            commentLibrary = commentList;
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
            comment = commentBox.Text;
            TextBox sentinel = sender as TextBox;
            if (commentLibrary.Count > 0 && sentinel.Text != "")
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(commentLibrary);
                view.Filter =
                    null;
                /*
                (o) =>
                {
                    //filter out all entries that neither start with nor contain the sentinel's comment
                    return (o as commentEntry).comment.Contains(sentinel.Text) ||
                        !(o as commentEntry).comment.StartsWith(sentinel.Text);
                };
                */
                if (!view.IsEmpty && !intelPopup.IsOpen)
                {
                    //kill the popup
                    intelPopup.Placement = PlacementMode.Left;
                    intelPopup.PlacementTarget = sentinel;
                    intelPopup.IsOpen = true;
                }
                else if (view.IsEmpty)
                {
                    intelPopup.IsOpen = false;
                }
            }
            else if (intelPopup != null && intelPopup.IsOpen)
            {
                intelPopup.IsOpen = false;
            }
        }
        private void commentConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            RoutedEventArgs newCommentEventArgs = new RoutedEventArgs(newCommentEvent, this);
            RaiseEvent(newCommentEventArgs);
        }
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            deleteAnimation();
        }
        private void deleteDoubleAnimation_Completed(object sender, EventArgs e)
        {
            raiseDeleteEvent();
        }
        private void detailsButton_Click(object sender, RoutedEventArgs e)
        {
            detailsExpanded = !detailsExpanded;
        }
        private void intelListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
                        RoutedEventArgs newCommentEventArgs = new RoutedEventArgs(newCommentEvent, this);
                        RaiseEvent(newCommentEventArgs);
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
        private void raiseDeleteEvent()
        {
            RoutedEventArgs newDeleteEvent = new RoutedEventArgs(deleteEvent, this);
            RaiseEvent(newDeleteEvent);
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