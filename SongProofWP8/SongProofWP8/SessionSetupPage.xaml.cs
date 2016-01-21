using SongProofWP8.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Phone.UI.Input;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace SongProofWP8
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SessionSetupPage : Page, INotifyPropertyChanged
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        bool ScaleGroupSelected = false;
        bool ScaleSelected = false;
        bool KeySelected = false;
        bool DifficultySelected = false;

        public event PropertyChangedEventHandler PropertyChanged;

        private int note_amount;

        public int NoteAmount
        {
            get
            {
                return note_amount;
            }
            set
            {
                if (note_amount != value)
                {
                    note_amount = value;
                    OnPropertyChanged("NoteAmount");
                }
            }
        }

        public SessionSetupPage()
        {
            this.InitializeComponent();
            CBScaleGroups.ItemsSource = ScaleResources.ScaleDivisionNames.Keys;
            CBDifficulty.ItemsSource = ScaleResources.DifficultyLevels;
            CBKey.ItemsSource = ScaleResources.PianoFlat;
            NoteAmount = ScaleResources.LOWEST_SET;
            CBScales.IsEnabled = false;

            DataContext = this;
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            HardwareButtons.BackPressed += GoBack;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
        private void GoBack(object sender, BackPressedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
                e.Handled = true;
            }
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
            if (ScaleGroupSelected && ScaleSelected && KeySelected && DifficultySelected)
            {
                Scale temp = ScaleResources.MakeScale((string)CBKey.SelectedValue,
                    (KVTuple<string, string>)CBScales.SelectedItem, (bool)ChckSharp.IsChecked);
                ScaleResources.Difficulties Diff = (ScaleResources.Difficulties)CBDifficulty.SelectedItem;
                SessionManager SM = new SessionManager(new Session(Diff, temp,
                    (bool)ChckSharp.IsChecked ? ScaleResources.PianoSharp : ScaleResources.PianoFlat,
                    ScaleResources.MakeQuiz(temp,
                    int.Parse(TBNoteCount.Text))));
                DataHolder.SM = SM;
                DataHolder.ShowSharp = (bool)ChckSharp.IsChecked;
                Frame.Navigate(typeof(ViewScale));
            }
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
                CBScales.ItemsSource = (List<KVTuple<string, string>>)ScaleResources.ScaleDivisionNames[(string)CBScaleGroups.SelectedValue];
            }
        }

        /// <summary>
        /// Registered to the ComboBox.SelectionChanged Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmSelection(object sender, SelectionChangedEventArgs e)
        {
            ComboBox sentinel = (ComboBox)sender;
            if (sentinel == CBScaleGroups)
            {
                ActivateScales();
                ScaleGroupSelected = true;
            }
            else if (sentinel == CBScales)
            {
                ScaleSelected = true;
            }
            else if (sentinel == CBKey)
            {
                KeySelected = true;
            }
            else if (sentinel == CBDifficulty)
            {
                DifficultySelected = true;
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
