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
        //All slider values are represented in MILLISECONDS
        private bool mediaPlayerIsPlaying = false;
        private bool userIsDraggingSlider = false;
        private const double _VOLUME_DELTA = .025;
        private const string DEFAULT_OFFSET_VALUE = "00:00:00.0000";

        private TimeSpan _videoDuration;
        private TimeSpan _audioDuration;

        private bool audioOffset = false;

        /// <summary>
        /// Represents the ProgressBar the mouse is over
        /// </summary>
        ProgressBar pbMouseOver;

        DispatcherTimer _vidTimer;
        DispatcherTimer _audTimer;
        DispatcherTimer _progressSlideTicker;

        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties
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

        private string _negSyncProgress;
        public string NegSyncProgress
        {
            get { return _negSyncProgress; }
            set
            {
                if (_negSyncProgress != value)
                {
                    _negSyncProgress = value;
                    NotifyPropertyChanged("NegSyncProgress");
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
        #endregion

        public MainWindow()
        {
            Offset = DEFAULT_OFFSET_VALUE;
            _vidTimer = new DispatcherTimer();
            _audTimer = new DispatcherTimer();
            _progressSlideTicker = new DispatcherTimer();
            _progressSlideTicker.Interval = TimeSpan.FromMilliseconds(1000);
            _progressSlideTicker.Tick += timer_Tick;

            /*
             * Doing the timer this way makes pausing wore more accurately:
             * http://stackoverflow.com/questions/3163300/what-is-the-different-between-isenabled-and-start-stop-of-dispatchertimer
             */
            _progressSlideTicker.Start();
            _progressSlideTicker.IsEnabled = false;
            InitializeComponent();
            DataContext = this;
            SyncProgress = "0.00";
        }

        #region Methods

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void SetSliderValues()
        {
            if ((videoPlayer != null && videoPlayer.Source != null) &&
                (audioPlayer != null && audioPlayer.Source != null))
            {
                MaxSlideValue = Math.Max(
                    (_audTimer.Interval.TotalMilliseconds + _audioDuration.TotalMilliseconds),
                    (_vidTimer.Interval.TotalMilliseconds + _videoDuration.TotalMilliseconds));
            }
            MinSlideValue = 0;
            SyncProgress = TimeSpan.FromMilliseconds(CurSlideValue).ToString();
            NegSyncProgress = TimeSpan.FromMilliseconds(sliSyncProgress.Maximum).ToString();
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
            SetSliderValues();
        }

        #region Event-based
        private void timer_Tick(object sender, EventArgs e)
        {
            if ((audioPlayer.Source != null) && (audioPlayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            {
                CurSlideValue += 1000;
                NegSyncProgress = TimeSpan.FromMilliseconds(sliSyncProgress.Maximum - CurSlideValue).ToString();
            }
        }

        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Open_ExecutedAudio(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select an Audio File";
            openFileDialog.Filter = "Media files (*.wav;*.mp3;*.wma; *.m4a; *.ogg)|*.wav;*.mp3;*.wma; *.m4a; *.ogg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                audioPlayer.Source = new Uri(openFileDialog.FileName);
            }
        }

        private void Open_ExecutedVideo(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a Video File";
            openFileDialog.Filter = "Video files (*.mp4; *.flv; *.avi; .mpg;*.mpeg)|*.mp4; *.flv; *.avi; .mpg;*.mpeg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                videoPlayer.Source = new Uri(openFileDialog.FileName);
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
            _progressSlideTicker.IsEnabled = true;
        }

        private void Pause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mediaPlayerIsPlaying;
        }

        private void Pause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            videoPlayer.Pause();
            audioPlayer.Pause();
            _progressSlideTicker.IsEnabled = false;
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
            _progressSlideTicker.Stop();
            _progressSlideTicker.Start();
            _progressSlideTicker.IsEnabled = false;
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
            SyncProgress = TimeSpan.FromMilliseconds(sliSyncProgress.Value).ToString();
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

        /// <summary>
        /// Registered TextBox.LostFocus Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TB_OffsetChanged(object sender, RoutedEventArgs e)
        {
            TextBox sentinel = sender as TextBox;
            TimeSpan result;
            if (TimeSpan.TryParse(sentinel.Text, out result))
            {
                Offset = result.ToString();
                UpdateOffsets();
            }
            else
            {
                Offset = DEFAULT_OFFSET_VALUE;
            }
        }
        private void TestClick(object sender, RoutedEventArgs e)
        {
            mediaPlayerIsPlaying = true;
            audioPlayer.Play();
            videoPlayer.Play();
            _progressSlideTicker.Start();
            SetSliderValues();
        }

        private void RecordVideoDuration(object sender, RoutedEventArgs e)
        {
            _videoDuration = videoPlayer.NaturalDuration.HasTimeSpan ? videoPlayer.NaturalDuration.TimeSpan : TimeSpan.Zero;
            SetSliderValues();
        }

        private void RecordAudioDuration(object sender, RoutedEventArgs e)
        {
            _audioDuration = audioPlayer.NaturalDuration.HasTimeSpan ? audioPlayer.NaturalDuration.TimeSpan : TimeSpan.Zero;
        }
        #endregion
        #endregion
    }
}
