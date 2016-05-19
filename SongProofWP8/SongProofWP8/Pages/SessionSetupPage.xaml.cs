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
    public sealed partial class SessionSetupPage : Page
    {
        private SessionSetupControl ssc = new SessionSetupControl();

        public SessionSetupPage()
        {
            this.InitializeComponent();

            TitleBarUserControl tbuc = new TitleBarUserControl("Setup");
            LayoutRoot.Children.Add(tbuc);
            Grid.SetRow(tbuc, 0);
            ssc.SetupNavigation("ToViewScalePage", this, this.GetType());
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            switch (DataHolder.ProofType)
            {
                case DataHolder.ProofingTypes.HW3:
                    ssc.HideKeys();
                    ssc.HideScaleGroups();
                    ssc.HideScales();
                    break;
                case DataHolder.ProofingTypes.ScaleWriting:
                default:
                    ssc.HideKeys();
                    break;
                case DataHolder.ProofingTypes.GrabBag:
                case DataHolder.ProofingTypes.PlacingTheNote:
                    break;
            }
            LayoutRoot.Children.Add(ssc);
            Grid.SetRow(ssc, 1);
        }


        private void ToViewScalePage()
        {
            bool valid = false;
            if (ssc.SelectedScaleGroup != null && ssc.SelectedScale != null &&
                 ssc.SelectedDifficulty != null)
            {
                switch (DataHolder.ProofType)
                {
                    case DataHolder.ProofingTypes.GrabBag:
                    case DataHolder.ProofingTypes.PlacingTheNote:
                        if (ssc.SelectedKey != null)
                        {
                            valid = true;
                        }
                        break;
                    case DataHolder.ProofingTypes.HW3:
                    case DataHolder.ProofingTypes.ScaleWriting:
                    default:
                        valid = true;
                        break;
                }
                if (valid)
                {
                    DataHolder.SetupTest((string)ssc.SelectedKey, (KVTuple<string, string>)ssc.SelectedScale, (bool)ssc.ShowSharp,
                        (ScaleResources.Difficulties)ssc.SelectedDifficulty, ssc.NoteAmount);
                    Frame.Navigate(typeof(ViewScale));
                }
            }
        }
    }
}
