using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace VideoAudioSyncer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        private bool mediaPlayerIsPlaying = false;
        private bool userIsDraggingSlider = false;
        private const double _VOLUME_DELTA = .025;

        /// <summary>
        /// Represents the ProgressBar the mouse is over
        /// </summary>
        ProgressBar pbMouseOver;

        public event PropertyChangedEventHandler PropertyChanged;

        private TimeSpan _syncProgress;
        public TimeSpan SyncProgress
        {
            get { return _syncProgress; }
            set
            {
                if (_syncProgress != value)
                {
                    _syncProgress = value;
                    NotifyPropertyChanged("SyncProgress");
                }
            }
        }

        DispatcherTimer _vidTimer;
        DispatcherTimer _audTimer;



        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _vidTimer = new DispatcherTimer();
            _audTimer = new DispatcherTimer();
        }
        #region Methods

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #region Event-based
        private void timer_Tick(object sender, EventArgs e)
        {
            if ((audioPlayer.Source != null) && (audioPlayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            {
                sliSyncProgress.Minimum = 0;
                sliSyncProgress.Maximum = audioPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                sliSyncProgress.Value = audioPlayer.Position.TotalSeconds;

            }
        }

        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Open_ExecutedAudio(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Media files (*.mp3;*.mpg;*.mpeg)|*.mp3;*.mpg;*.mpeg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
                audioPlayer.Source = new Uri(openFileDialog.FileName);
        }

        private void Open_ExecutedVideo(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Media files (*.mp3;*.mpg;*.mpeg)|*.mp3;*.mpg;*.mpeg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
                audioPlayer.Source = new Uri(openFileDialog.FileName);
        }

        private void Play_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = videoPlayer != null && videoPlayer.Source != null;
        }

        private void Play_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mediaPlayerIsPlaying = true;
        }

        private void Pause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mediaPlayerIsPlaying;
        }

        private void Pause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            videoPlayer.Pause();
            audioPlayer.Pause();
            mediaPlayerIsPlaying = false;
        }

        private void Stop_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mediaPlayerIsPlaying;
        }

        private void Stop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            videoPlayer.Stop();
            audioPlayer.Stop();
            mediaPlayerIsPlaying = false;
        }

        private void sliProgress_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void sliProgress_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            videoPlayer.Position = TimeSpan.FromSeconds(sliSyncProgress.Value);
            audioPlayer.Position = TimeSpan.FromSeconds(sliSyncProgress.Value);
        }

        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SyncProgress = sliSyncProgress.Value.ToString("hh:mm:ss");
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            MediaElement sentinel = audioPlayer;

            double delta = e.Delta > 0 ? 0.1 : -0.1;

            Point mousePos = e.MouseDevice.GetPosition(pbAudVolume);
            if (pbMouseOver != pbSyncVolume)
            {
                if (pbMouseOver == pbAudVolume)
                {
                    sentinel = audioPlayer;
                }
                else if (pbMouseOver == pbVidVolume)
                {
                    sentinel = videoPlayer;
                }
                else
                {
                    delta = 0;
                }
                if (sentinel.Volume + delta >= 0 && sentinel.Volume + delta <= 1)
                {
                    sentinel.Volume += delta;
                }
                pbSyncVolume.Value = Math.Min(audioPlayer.Volume, videoPlayer.Volume);
            }
            else
            {
                pbSyncVolume.Value += delta;
                audioPlayer.Volume = pbSyncVolume.Value;
                videoPlayer.Volume = pbSyncVolume.Value;
            }
        }

        private void SaveProfile_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        private void SaveProfile_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void pb_MouseEnter(object sender, MouseEventArgs e)
        {
            ProgressBar sentinel = sender as ProgressBar;
            if (sentinel != null)
            {
                pbMouseOver = sentinel;
            }
        }

        private void pb_MouseLeave(object sender, MouseEventArgs e)
        {
            pbMouseOver = null;
        }
        #endregion
        #endregion
    }
}
