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

        private bool audioOffset = false;

        /// <summary>
        /// Represents the ProgressBar the mouse is over
        /// </summary>
        ProgressBar pbMouseOver;

        DispatcherTimer _vidTimer;
        DispatcherTimer _audTimer;

        public event PropertyChangedEventHandler PropertyChanged;

        private string _offset;
        public string Offset
        {
            get { return _offset; }
            set
            {
                if (_offset != value)
                {
                    _offset = value;
                    NotifyPropertyChanged("Offset");
                }
            }
        }

        private string _syncProgress;
        public string SyncProgress
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

        private double _maxSlideValue;
        public double MaxSlideValue
        {
            get { return _maxSlideValue; }
            set
            {
                if (_maxSlideValue != value)
                {
                    _maxSlideValue = value;
                    NotifyPropertyChanged("MaxSlideValue");
                }
            }
        }

        private double _minSlideValue;
        public double MinSlideValue
        {
            get { return _minSlideValue; }
            set
            {
                if (_minSlideValue != value)
                {
                    _minSlideValue = value;
                    NotifyPropertyChanged("MinSlideValue");
                }
            }
        }

        private double _curSlideValue;
        public double CurSlideValue
        {
            get { return _curSlideValue; }
            set
            {
                if (_curSlideValue != value)
                {
                    _curSlideValue = value;
                    NotifyPropertyChanged("CurSlideValue");
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _vidTimer = new DispatcherTimer();
            _audTimer = new DispatcherTimer();
            SyncProgress = "0.00";
            Offset = "00:00:00.0000";
        }
        #region Methods

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void SetMaxSliderValue()
        {
            if ((videoPlayer != null && videoPlayer.Source != null) &&
                (audioPlayer != null && audioPlayer.Source != null))
            {
                MaxSlideValue = Math.Max(
                    (_audTimer.Interval.TotalMilliseconds +
                    (audioPlayer.NaturalDuration.HasTimeSpan ? audioPlayer.NaturalDuration.TimeSpan.TotalMilliseconds : 0)),

                    (_vidTimer.Interval.TotalMilliseconds +
                    (videoPlayer.NaturalDuration.HasTimeSpan ? videoPlayer.NaturalDuration.TimeSpan.TotalMilliseconds : 0))
                    );
            }
            MinSlideValue = 0;
            CurSlideValue = 0;
        }

        private void UpdateOffsets()
        {
            if (audioOffset)
            {
                _vidTimer.Interval = TimeSpan.Zero;
                _audTimer.Interval = TimeSpan.Parse(Offset);
            }
            else
            {
                _audTimer.Interval = TimeSpan.Zero;
                _vidTimer.Interval = TimeSpan.Parse(Offset);
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
            openFileDialog.Filter = "Media files (*.wav;*.mp3;*.wma)|*.wav;*.mp3;*.wma|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                audioPlayer.Source = new Uri(openFileDialog.FileName);
                SetMaxSliderValue();
            }
        }

        private void Open_ExecutedVideo(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Media files (*.mp3;*.mpg;*.mpeg)|*.mp3;*.mpg;*.mpeg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                videoPlayer.Source = new Uri(openFileDialog.FileName);
                SetMaxSliderValue();
            }
        }

        private void Play_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (videoPlayer != null && videoPlayer.Source != null) &&
                (audioPlayer != null && audioPlayer.Source != null);
        }

        private void Play_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mediaPlayerIsPlaying = true;
            audioPlayer.Play();
            videoPlayer.Play();
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
            videoPlayer.Position = TimeSpan.FromMilliseconds(sliSyncProgress.Value);
            audioPlayer.Position = TimeSpan.FromMilliseconds(sliSyncProgress.Value);
        }

        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SyncProgress = Math.Round(sliSyncProgress.Value, 2).ToString("F2");
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
                pbSyncVolume.Value = Math.Round((audioPlayer.Volume + videoPlayer.Volume) / 2, 3);
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
            e.CanExecute = ((videoPlayer != null && videoPlayer.Source != null) &&
                (audioPlayer != null && audioPlayer.Source != null));
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

        private void RB_AudioChecked(object sender, RoutedEventArgs e)
        {
            audioOffset = true;
            UpdateOffsets();
        }

        private void RB_VideoChecked(object sender, RoutedEventArgs e)
        {
            audioOffset = false;
            UpdateOffsets();
        }

        private void TB_OffsetChanged(object sender, RoutedEventArgs e)
        {
            TextBox sentinel = sender as TextBox;
            TimeSpan result;
            TimeSpan.TryParse(sentinel.Text, out result);
            Offset = result.ToString();
            UpdateOffsets();
        }
        #endregion
        #endregion
    }
}
