using SongProofWP8.UserControls;
using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace SongProofWP8.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HW3SetupPage : Page
    {
        public HW3SetupPage()
        {
            this.InitializeComponent();
            SessionSetupControl ssu = new SessionSetupControl(Visibility.Collapsed, Visibility.Collapsed,
                Visibility.Collapsed, Visibility.Visible, Visibility.Visible);
            LayoutRoot.Children.Add(ssu);

            ssu.SetupNavigation("StartMethod", this,this.GetType());

            Grid.SetRow(ssu, 1);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void StartMethod()
        {

        }
    }
}
