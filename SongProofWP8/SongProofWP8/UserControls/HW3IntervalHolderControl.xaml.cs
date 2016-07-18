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
    public sealed partial class HW3IntervalHolderControl : UserControl, INotifyPropertyChanged
    {
        private string note1Var;
        private string note2Var;


        public string Note1Var
        {
            get { return note1Var; }
            set
            {
                if (note1Var != value)
                {
                    note1Var = value;
                    NotifyPropertyChanged("Note1Var");
                }
            }
        }
        public string Note2Var
        {
            get { return note2Var; }
            set
            {
                if (note2Var != value)
                {
                    note2Var = value;
                    NotifyPropertyChanged("Note2Var");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public HW3IntervalHolderControl()
        {
            this.InitializeComponent();
        }
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
