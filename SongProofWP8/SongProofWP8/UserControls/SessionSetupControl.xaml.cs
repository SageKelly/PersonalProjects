using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
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
    /// <summary>
    /// All the Scale-deciding stuff for each testing type
    /// </summary>
    public sealed partial class SessionSetupControl : UserControl, INotifyPropertyChanged
    {

        private MethodInfo _startMethod;
        private object _startMethodTarget;

        public event PropertyChangedEventHandler PropertyChanged;

        private int _noteAmount;
        public int NoteAmount
        {
            get
            {
                return _noteAmount;
            }
            set
            {
                if (_noteAmount != value)
                {
                    _noteAmount = value;
                    OnPropertyChanged("NoteAmount");
                }
            }
        }

        private string _selectedScaleGroup;
        public string SelectedScaleGroup
        {
            get
            {
                return _selectedScaleGroup;
            }
            set
            {
                if (_selectedScaleGroup != value)
                {
                    _selectedScaleGroup = value;
                    OnPropertyChanged("SelectedScaleGroup");
                }
            }
        }


        private object _selectedScale;
        public object SelectedScale
        {
            get
            {
                return _selectedScale;
            }
            set
            {
                if (_selectedScale != value)
                {
                    _selectedScale = value;
                    OnPropertyChanged("SelectedScale");
                }
            }
        }

        private string _selectedKey;
        public string SelectedKey
        {
            get
            {
                return _selectedKey;
            }
            set
            {
                if (_selectedKey != value)
                {
                    _selectedKey = value;
                    OnPropertyChanged("SelectedKey");
                }
            }
        }

        private ScaleResources.Difficulties _selectedDifficulty;
        public ScaleResources.Difficulties SelectedDifficulty
        {
            get
            {
                return _selectedDifficulty;
            }
            set
            {
                if (_selectedDifficulty != value)
                {
                    _selectedDifficulty = value;
                    OnPropertyChanged("SelectedDifficulty");
                }
            }
        }

        private bool _showSharp;
        public bool ShowSharp
        {
            get
            {
                return _showSharp;
            }
            set
            {
                if (_showSharp != value)
                {
                    _showSharp = value;
                    OnPropertyChanged("ShowSharp");
                }
            }
        }

        /// <summary>
        /// Instantiates a SessionSetupUserControl
        /// </summary>
        /// <param name="showScaleGroups">Should the Scale Groups ComboBox show?</param>
        /// <param name="showScales">Should the Scales ComboBox show?</param>
        /// <param name="showScaleKey">Should the Scale ComboBox show?</param>
        /// <param name="showDifficulty">Should the Difficulty ComboBox show?</param>
        /// <param name="showNoteAmount">Should the Note Amount UserControl show?</param>
        public SessionSetupControl(string methodName, object target, Type targetType,
            bool showScaleGroups = true, bool showScales = true, bool showScaleKey = true)
        {
            this.InitializeComponent();

            TypeInfo classTypeInfo = targetType.GetTypeInfo();
            MethodInfo method = classTypeInfo.GetDeclaredMethod(methodName);

            _startMethod = method;
            _startMethodTarget = target;

            CBScaleGroups.ItemsSource = ScaleResources.ScaleDivisionNames.Keys;
            CBDifficulty.ItemsSource = ScaleResources.DifficultyLevels;
            CBKey.ItemsSource = ScaleResources.PianoFlat;
            NoteAmount = ScaleResources.LOWEST_SET;

            //disabled until scale group is chosen
            CBScales.IsEnabled = false;

            CBScaleGroups.Visibility = showScaleGroups ? Visibility.Visible : Visibility.Collapsed;
            CBScales.Visibility = showScales ? Visibility.Visible : Visibility.Collapsed;
            SPScaleKey.Visibility = showScaleKey ? Visibility.Visible : Visibility.Collapsed;

            DataContext = this;
        }

        private void ChckSharp_Checked(object sender, RoutedEventArgs e)
        {
            int index = -1;
            if (CBKey.SelectedIndex != -1)
                index = CBKey.SelectedIndex;
            CBKey.ItemsSource = ScaleResources.PianoSharp;
            CBKey.SelectedIndex = index;
        }

        private void ChckSharp_Unchecked(object sender, RoutedEventArgs e)
        {
            int index = -1;
            if (CBKey.SelectedIndex != -1)
                index = CBKey.SelectedIndex;
            CBKey.ItemsSource = ScaleResources.PianoFlat;
            CBKey.SelectedIndex = index;
        }

        private void BStart_Click(object sender, RoutedEventArgs e)
        {
            _startMethod.Invoke(_startMethodTarget, new object[1] { sender });

        }

        /// <summary>
        /// Rigged to the CBScaleGroups.SelectionChanged Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActivateScales()
        {
            if (CBScaleGroups.SelectedIndex != -1)
            {
                CBScales.IsEnabled = true;
                CBScales.ItemsSource = ScaleResources.ScaleDivisionNames[(string)CBScaleGroups.SelectedValue];
            }
        }

        #region Show/Hides
        public void ShowKeys()
        {
            CBKey.Visibility = Visibility.Visible;
        }

        public void HideKeys()
        {
            CBKey.Visibility = Visibility.Collapsed;
        }

        public void ShowScaleGroups()
        {
            CBScaleGroups.Visibility = Visibility.Visible;
        }

        public void HideScaleGroups()
        {
            CBScaleGroups.Visibility = Visibility.Collapsed;
        }

        public void ShowScales()
        {
            CBScales.Visibility = Visibility.Visible;
        }

        public void HideScales()
        {
            CBScales.Visibility = Visibility.Collapsed;
        }

        #endregion



        /// <summary>
        /// Registered to the ComboBox.SelectionChanged Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmSelection(object sender, SelectionChangedEventArgs e)
        {
            ComboBox sentinel = sender as ComboBox;
            if (sentinel != null)
            {
                ActivateScales();
            }
        }

        private void IncDecValue(object sender, RoutedEventArgs e)
        {
            Button sentinel = (Button)sender;
            if (sentinel == UpButton && NoteAmount < ScaleResources.HIGHEST_SET)
            {
                NoteAmount += ScaleResources.LOWEST_INC;
            }
            else if (sentinel == DownButton && NoteAmount > ScaleResources.LOWEST_SET)
            {
                NoteAmount -= ScaleResources.LOWEST_INC;
            }
        }

        private void OnPropertyChanged(string property_name_)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property_name_));
        }
    }
}
