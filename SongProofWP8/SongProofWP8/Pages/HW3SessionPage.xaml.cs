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
    public sealed partial class HW3SessionPage : Page
    {
        /// <summary>
        /// Represents the current index within the quiz array
        /// </summary>
        private int curIndex;
        /// <summary>
        /// Represents the list of intervals
        /// </summary>
        private int[] quiz;

        /// <summary>
        /// Represents the Note value to update within the display control
        /// </summary>
        private int switching_value;

        private string timer_text;
        private int note_number;
        private static string TIMER_FLAVOR = "Time left: ";
        private bool countingDown;
        private bool SessionStarted = false;
        private int remainingTime;
        private DispatcherTimer TickDownTimer;

        private const string IH = "H";
        private const string IW = "W";
        private const string I3 = "-3";

        public HW3SessionPage()
        {
            this.InitializeComponent();
            TitleBarControl tbuc = new TitleBarControl("HW3 Proofing");
            LayoutRoot.Children.Add(tbuc);
            Grid.SetRow(tbuc, 0);

            HW3DisplayControl hdc = new HW3DisplayControl();
            LayoutRoot.Children.Add(hdc);
            Grid.SetRow(hdc, 1);

            HW3Control hw3 = new HW3Control(IH, IW, I3, "IntervalClick", this, typeof(HW3SessionPage));
            LayoutRoot.Children.Add(hw3);
            Grid.SetRow(hw3, 2);

            curIndex = 0;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void IntervalClick(object sender)
        {
            Button sentinel = sender as Button;
            string content = sentinel.Content.ToString();

            int interval = 0;

            switch (content)
            {
                case IH:
                    interval = 1;
                    break;
                case IW:
                    interval = 2;
                    break;
                case I3:
                    interval = 3;
                    break;
            }



        }
    }
}
