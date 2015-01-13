using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClockWatcher
{
    [Serializable]
    public class Session : DependencyObject
    {
        public static readonly DependencyProperty nameProperty =
            DependencyProperty.Register("name", typeof(string),
            typeof(Session), new FrameworkPropertyMetadata(string.Empty));

        public static readonly DependencyProperty totalTimeProperty =
            DependencyProperty.Register("totalTime", typeof(TimeSpan), typeof(Session),
            new PropertyMetadata(TimeSpan.Zero));

        public List<TimeEntry> timeEntries { get; private set; }
        public TimeEntry currentTimeEntry { get; private set; }
        public DateTime creationDate { get; private set; }
        public TimeSpan totalTime
        {
            get
            {
                return (TimeSpan)GetValue(totalTimeProperty);
            }
            set
            {
                SetValue(totalTimeProperty, value);
            }
        }

        public string name
        {
            get
            {
                return (string)GetValue(nameProperty);
            }
            set
            {
                SetValue(nameProperty, value);
            }
        }

        public Session()
        {
            timeEntries = new List<TimeEntry>();
            creationDate = DateTime.Now;
            name = DateTime.Now.ToString();
        }

        public void addEntry(DateTime timeIn)
        {
            timeEntries.Add(new TimeEntry());
            currentTimeEntry = timeEntries.Last();
            currentTimeEntry.timeIn = timeIn;
            currentTimeEntry.delete += delete;
        }

        private void delete(object sender, System.Windows.RoutedEventArgs e)
        {
            timeEntries.Remove(sender as TimeEntry);
            if (timeEntries.Count > 0)
                currentTimeEntry = timeEntries.Last();
            else
                currentTimeEntry = null;
        }


    }
}
