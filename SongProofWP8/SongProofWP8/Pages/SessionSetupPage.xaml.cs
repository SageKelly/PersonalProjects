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
        private SessionSetupControl _ssc;

        public SessionSetupPage()
        {
            this.InitializeComponent();
            _ssc = new SessionSetupControl("ToViewScalePage", this, typeof(SessionSetupPage));

            TitleBarControl tbuc = new TitleBarControl("Setup");
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
            switch (DataHolder.ProofType)
            {
                case DataHolder.ProofingTypes.HW3:
                    _ssc.HideKeys();
                    _ssc.HideScaleGroups();
                    _ssc.HideScales();
                    break;
                case DataHolder.ProofingTypes.ScaleWriting:
                default:
                    _ssc.HideKeys();
                    break;
                case DataHolder.ProofingTypes.GrabBag:
                case DataHolder.ProofingTypes.PlacingTheNote:
                    break;
            }
            LayoutRoot.Children.Add(_ssc);
            Grid.SetRow(_ssc, 1);
        }


        private void ToViewScalePage(object sender)
        {
            bool valid = false;
            switch (DataHolder.ProofType)
            {
                case DataHolder.ProofingTypes.GrabBag:
                case DataHolder.ProofingTypes.PlacingTheNote:
                    if (_ssc.SelectedScaleGroup != null && _ssc.SelectedScale != null && _ssc.SelectedKey != null)
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
                switch (DataHolder.ProofType)
                {
                    case DataHolder.ProofingTypes.PlacingTheNote:
                        DataHolder.SetupPTNTest(DataHolder.ProofType, (string)_ssc.SelectedKey, (KVTuple<string, string>)_ssc.SelectedScale, (bool)_ssc.ShowSharp,
                            (ScaleResources.Difficulties)_ssc.SelectedDifficulty, _ssc.NoteAmount);
                        Frame.Navigate(typeof(ViewScale));
                        break;
                    case DataHolder.ProofingTypes.HW3:
                        DataHolder.SetupHW3Test(DataHolder.ProofType, _ssc.ShowSharp, (ScaleResources.Difficulties)_ssc.SelectedDifficulty, _ssc.NoteAmount);
                        Frame.Navigate(typeof(SessionPage));
                        break;
                }
            }
        }
    }
}
