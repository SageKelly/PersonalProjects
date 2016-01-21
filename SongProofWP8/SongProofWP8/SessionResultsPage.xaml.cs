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

namespace SongProofWP8
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SessionResultsPage : Page
    {

        Dictionary<string, NoteAnalytics> Analysis;

        public SessionResultsPage()
        {
            this.InitializeComponent();
            Analysis = new Dictionary<string, NoteAnalytics>();
            RunAnalytics();
            DataContext = Analysis.Values;
            //LV_Results.ItemsSource = Analysis;
        }

        public void RunAnalytics()
        {
            Session curSession = DataHolder.SM.CurrentSession;
            string[] scale = curSession.ScaleUsed.Notes;
            foreach (string s in scale)
            {
                Analysis.Add(s, new NoteAnalytics(s));
            }
            foreach (Session.NoteData nd in curSession.Data)
            {
                NoteAnalytics na = Analysis[scale[nd.NoteIndex]];
                na.Count++;
                na.AvgGuessingTime += nd.GuessTime;//we're just collecting them here
                na.CorrectGuesses += nd.Correct ? 1 : 0;
                Analysis[scale[nd.NoteIndex]] = na;
            }
            foreach (KeyValuePair<string, NoteAnalytics> na in Analysis)
            {
                na.Value.AvgGuessingTime = na.Value.Count == 0 ? 0 : Math.Round(((na.Value.AvgGuessingTime / na.Value.Count) / 1000), 2);
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void RevealNoteInfo(object sender, RoutedEventArgs e)
        {

        }

        private void RestartSession(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ViewScale));
        }

        private void ToMain(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
