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
        //Video media is ALWAYS the last media element to be manipulated
        private bool mediaPlayerIsPlaying = false;
        private bool userIsDraggingSlider = false;
        private const double _VOLUME_DELTA = .025;
        private const string DEFAULT_OFFSET_VALUE = "00:00:00.0000";

        /// <summary>
        /// Represents the duration of the video media
        /// </summary>
        private TimeSpan _videoDuration;
        /// <summary>
        /// Represents the duration of the audio media
        /// </summary>
        private TimeSpan _audioDuration;

        /// <summary>
        /// Represents whether or not the offset dictated corresponds
        /// to the audio media or the video
        /// </summary>
        private bool audioOffset = false;

        /// <summary>
        /// Represents the ProgressBar the mouse is over
        /// </summary>
        ProgressBar pbMouseOver;

        /// <summary>
        /// The offset timer for the the video media
        /// </summary>
        DispatcherTimer _vidTimer;
        /// <summary>
        /// The offse timer for the audio media
        /// </summary>
        DispatcherTimer _audTimer;
        /// <summary>
        /// The timer that updates the values for SyncProgress and NegSyncProgress
        /// </summary>
        DispatcherTimer _progressSlideTicker;

        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties
        private string _offset;
        /// <summary>
        /// The offset delegated to either the audio or video element, pending on the value of audioOffset
        /// </summary>
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
        /// <summary>
        /// The text representation of the overall progress of both the audio and video media elements
        /// </summary>
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
        /// <summary>
        /// The text representation of the remaining time left for the audio and video media elements
        /// </summary>
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
        /// <summary>
        /// Represents the maximum value, in milliseconds, for the synced progress
        /// </summary>
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
        /// <summary>
        /// Represents the minimum value, in miiliseconds, for the synced progress
        /// </summary>
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
        /// <summary>
        /// Represents the current value, in milliseconds, for the synced progress
        /// </summary>
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
            _vidTimer.Tick += vidTick;
            _audTimer.Tick += audTick;
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

        private void vidTick(object sender, EventArgs e)
        {
            DispatcherTimer timer = sender as DispatcherTimer;
            if (timer != null)
            {
                timer.Stop();
                videoPlayer.Play();
            }
        }

        private void audTick(object sender, EventArgs e)
        {
            DispatcherTimer timer = sender as DispatcherTimer;
            if (timer != null)
            {
                timer.Stop();
                audioPlayer.Play();
            }
        }

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
        }

        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SyncProgress = TimeSpan.FromMilliseconds(CurSlideValue).ToString();
            audioPlayer.Position = TimeSpan.FromMilliseconds(CurSlideValue - _audTimer.Interval.TotalMilliseconds);
            videoPlayer.Position = TimeSpan.FromMilliseconds(CurSlideValue - _vidTimer.Interval.TotalMilliseconds);
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
            StringBuilder text = new StringBuilder(sentinel == null ? string.Empty : sentinel.Text);
            string temp = string.Empty;
            if (text[0] == '-')
            {
                temp = text.ToString().Substring(1);
            }
            else
            {
                temp = text.ToString();
            }

            if (audioOffset)
            {
                _vidTimer.Stop();
                _vidTimer.Interval = TimeSpan.Zero;
                _audTimer.Interval = TimeSpan.Parse(temp);
            }
            else
            {
                _audTimer.Stop();
                _audTimer.Interval = TimeSpan.Zero;
                _vidTimer.Interval = TimeSpan.Parse(temp);
            }
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

        /// <summary>
        /// Inserts data into the textbox without moving the punctuation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TB_Offset_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox sentinel = sender as TextBox;
            StringBuilder text = new StringBuilder(string.Empty);
            bool isNumber = false;
            if (sentinel != null)
            {
                text.Clear();
                text.Append(sentinel.Text);
                char insertChar = ' ';
                switch (e.Key)
                {
                    case Key.NumPad0:
                    case Key.D0:
                        insertChar = '0';
                        isNumber = true;
                        break;
                    case Key.NumPad1:
                    case Key.D1:
                        insertChar = '1';
                        isNumber = true;
                        break;
                    case Key.NumPad2:
                    case Key.D2:
                        insertChar = '2';
                        isNumber = true;
                        break;
                    case Key.NumPad3:
                    case Key.D3:
                        insertChar = '3';
                        isNumber = true;
                        break;
                    case Key.NumPad4:
                    case Key.D4:
                        insertChar = '4';
                        isNumber = true;
                        break;
                    case Key.NumPad5:
                    case Key.D5:
                        insertChar = '5';
                        isNumber = true;
                        break;
                    case Key.NumPad6:
                    case Key.D6:
                        insertChar = '6';
                        isNumber = true;
                        break;
                    case Key.NumPad7:
                    case Key.D7:
                        insertChar = '7';
                        isNumber = true;
                        break;
                    case Key.NumPad8:
                    case Key.D8:
                        insertChar = '8';
                        isNumber = true;
                        break;
                    case Key.NumPad9:
                    case Key.D9:
                        insertChar = '9';
                        isNumber = true;
                        break;
                    case Key.Back:
                        if (sentinel.CaretIndex == 0 && text[sentinel.CaretIndex] == '-')
                        {
                            text.Remove(sentinel.CaretIndex, 1);
                        }
                        else if (sentinel.CaretIndex == 0)
                        {
                            text[sentinel.CaretIndex] = '0';
                        }
                        else if (sentinel.CaretIndex != 0 &&
                            (text[sentinel.CaretIndex - 1] == ':' || text[sentinel.CaretIndex - 1] == '.'))
                        {
                            sentinel.CaretIndex--;
                            text[sentinel.CaretIndex] = '0';
                            sentinel.CaretIndex--;
                        }
                        break;
                    case Key.Delete:
                        if (text[sentinel.CaretIndex - 1] == ':' || text[sentinel.CaretIndex - 1] == '.')
                        {
                            sentinel.CaretIndex++;
                        }
                        text[sentinel.CaretIndex] = '0';
                        break;
                    case Key.OemMinus:
                        text.Insert(0, '-');
                        break;
                    case Key.OemPlus:
                        if (text[0] == '-')
                        {
                            text.Remove(0, 1);
                        }
                        break;
                    case Key.Enter:
                        this.Focus();
                        break;
                }
                if (isNumber)
                {
                    if (text[sentinel.CaretIndex] == ':')
                    {
                        sentinel.CaretIndex++;
                    }
                    text[sentinel.CaretIndex] = insertChar;
                }
            }
        }
        #endregion
    }
}
