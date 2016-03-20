using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SongProofWP8.UserControls
{
    public sealed partial class ProgressTrackerControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _minValue;
        public int MinValue
        {
            get
            {
                return _minValue;
            }
            set
            {
                if (_minValue != value)
                {
                    _minValue = value;
                    NotifyPropertyChanged("MinValue");
                }
            }
        }

        private int _maxValue;
        public int MaxValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                if (_maxValue != value)
                {
                    _maxValue = value;
                    NotifyPropertyChanged("MaxValue");
                }
            }
        }

        private string _testingLabel;
        public string TestingLabel
        {
            get { return _testingLabel; }
            set
            {
                if (_testingLabel != value)
                {
                    _testingLabel = value;
                    NotifyPropertyChanged("TestingLabel");
                }
            }
        }

        private string _testingValue;
        public string TestingValue
        {
            get { return _testingValue; }
            set
            {
                if (_testingValue != value)
                {
                    _testingValue = value;
                    NotifyPropertyChanged("TestingValue");
                }
            }
        }

        private double _pBValue;
        public double PBValue
        {
            get { return _pBValue; }
            set
            {
                if (_pBValue != value)
                {
                    _pBValue = value;
                    NotifyPropertyChanged("PBValue");
                }
            }
        }

        /// <summary>
        /// Makes a ProgressTrackerControl to visually represent the user's progress
        /// </summary>
        /// <param name="testingLabel">The label to be printed
        /// above the place showing the testing value</param>
        public ProgressTrackerControl(string testingLabel)
        {
            this.InitializeComponent();
            TestingLabel = testingLabel;
            DataContext = this;
        }


        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Displays a checkmark or an X depending on the parameter value
        /// </summary>
        /// <param name="correct">If true, a checkmark, else an X shows</param>
        public void ShowResultPic(bool correct)
        {
            if (correct)
            {
                XBorder.Visibility = Visibility.Collapsed;
                CheckBorder.Visibility = Visibility.Visible;
                FadeInCheck.Begin();
            }
            else
            {
                CheckBorder.Visibility = Visibility.Collapsed;
                XBorder.Visibility = Visibility.Visible;
                FadeInX.Begin();
            }
        }
    }
}
