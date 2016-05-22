using SongProofWP8.UserControls;
using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace SongProofWP8.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ScaleProofTestListPage : Page
    {
        public ScaleProofTestListPage()
        {
            this.InitializeComponent();

            TitleBarControl tbuc = new TitleBarControl("Concentrate on...", 53.5);
            LayoutRoot.Children.Add(tbuc);
            Grid.SetRow(tbuc, 0);            
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void ToHW3Setup(object sender, RoutedEventArgs e)
        {
            DataHolder.ProofType = DataHolder.ProofingTypes.HW3;
            Frame.Navigate(typeof(SessionSetupPage));
        }

        private void ToPlacingTheNoteSetup(object sender, RoutedEventArgs e)
        {
            DataHolder.ProofType = DataHolder.ProofingTypes.PlacingTheNote;
            Frame.Navigate(typeof(SessionSetupPage));
        }
    }
}
