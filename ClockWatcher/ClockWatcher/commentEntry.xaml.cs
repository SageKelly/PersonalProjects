using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Interaction logic for commentEntry.xaml
    /// </summary>
    public partial class commentEntry : UserControl
    {

        public static readonly DependencyProperty commentProperty =
            DependencyProperty.Register("comment", typeof(string),
            typeof(commentEntry), new PropertyMetadata(""));

        public static readonly RoutedEvent deleteEvent = EventManager.RegisterRoutedEvent("delete",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(commentEntry));

        private TransformGroup animatedTransform;
        private ScaleTransform animatedScale;

        private DoubleAnimation scaleAnim;

        public string comment
        {
            get
            {
                return (string)GetValue(commentProperty);
            }
            set
            {
                SetValue(commentProperty, value);
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

        public commentEntry()
        {
            this.InitializeComponent();
            scaleAnim = new DoubleAnimation();
            animatedTransform = new TransformGroup();
            animatedScale = new ScaleTransform();
            animatedTransform.Children.Add(animatedScale);
            this.RenderTransform = animatedTransform;
            scaleAnim.Completed += animationScale_Completed;
        }

        void animationScale_Completed(object sender, EventArgs e)
        {
            RoutedEventArgs newDeleteEvent = new RoutedEventArgs(deleteEvent, this);
            RaiseEvent(newDeleteEvent);
        }

        public commentEntry(string comment)
            : this()
        {
            this.comment = comment;
        }

        private void deleteAnimation()
        {
            scaleAnim.To = 0;
            scaleAnim.Duration = new Duration(TimeSpan.Parse("0:0:.25"));
            animatedScale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnim);
            animatedScale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnim);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            deleteAnimation();
        }

    }
}