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
    public sealed partial class TitleBarControl : UserControl, INotifyPropertyChanged
    {
        string _textValue;
        double _fontSizeValue;
        const double DEFAULT_FONT_SIZE = 57.5;
        public string TextValue
        {
            get
            {
                return _textValue;
            }
            set
            {
                if (_textValue != value)
                {
                    _textValue = value;
                    NotifyPropertyChanged("TextValue");
                }
            }
        }
        public double FontSizeValue
        {
            get
            {
                return _fontSizeValue;
            }
            set
            {
                if (_fontSizeValue != value)
                {
                    _fontSizeValue = value;
                    NotifyPropertyChanged("FontSizeValue");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public TitleBarControl(string text, double fontSize = DEFAULT_FONT_SIZE)
        {
            this.InitializeComponent();
            TextValue = text;
            FontSizeValue = fontSize;
            DataContext = this;
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
