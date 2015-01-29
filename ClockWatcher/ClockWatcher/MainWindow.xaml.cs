using System;
using System.Collections.Generic;
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
using System.Timers;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Xml.Serialization;
using System.IO;

namespace ClockWatcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string SESSION_FILENAME = "SessionFiles.xml";

        /// <summary>
        /// Represents, during selection mode, which TimeEntry is currently selected.
        /// </summary>
        private TimeEntry currentSelectedEntry;

        private int selectionIndex;
        public string defaultComment { get; private set; }

        private bool _isSelecting;
        private Binding binding;
        private TextBox _currentTextbox;

        public SessionManager SM { get; private set; }

        #region Properties
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            SM = new SessionManager();
            SM.newAddedCommentEvent += currentTimeEntry_newComment;
            SM.timeEntryDeletedEvent += currentTimeEntry_delete;
            SM.commentEntryDeletedEvent += entry_delete;
            //Deserialize();
            defaultComment = "Add New Comment";
            commentAddingBox.Text = defaultComment;
            _isSelecting = false;
            //this.Closed += MainWindow_Closed;
            DataContext = SM;
        }


        #region Methods
        #region Event Methods
        /// <summary>
        /// Registered to MainWindow.commentAddingButton.Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void commentAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (commentAddingBox.Text != defaultComment && commentAddingBox.Text != "")
            {
                SM.newCommentEntry(commentAddingBox.Text);
            }
        }
        /// <summary>
        /// Registered to MainWindow.commentAddingBox.GotFocus Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void commentAddingBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).Text = "";
        }
        /// <summary>
        /// Registered to MainWindow.commentAddingBox.LostFocus Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void commentAddingBox_LostFocus(object sender, RoutedEventArgs e)
        {
            commentAddingBox.Text = defaultComment;
        }
        /// <summary>
        /// Registered to TimeEntry.textChangedEvent Event
        /// (see entryAdder_Click())
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void commentBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _currentTextbox = sender as TextBox;

            binding = new Binding();
            binding.Source = SM.commentLibrary;
            intelListBox.SetBinding(ListBox.ItemsSourceProperty, binding);

            ICollectionView view = CollectionViewSource.GetDefaultView(SM.commentLibrary);
            view.Filter =
                //null;
            (o) =>
            {
                //filter out all entries that neither start with nor contain the sentinel's comment
                if (_currentTextbox.Text == string.Empty)
                {
                    return (o as commentEntry).comment != string.Empty;
                }
                else
                {
                    return (o as commentEntry).comment.Contains(_currentTextbox.Text);
                }
            };
            /*
            */
            if (_currentTextbox.IsFocused)
            {
                intelPopup.Placement = PlacementMode.Left;
                intelPopup.PlacementTarget = _currentTextbox;
                intelPopup.IsOpen = true;
            }
        }
        /// <summary>
        /// Registered to SessionManager.timeEntryDeletedEvent Event
        /// </summary>
        /// <param name="deletionIndex"></param>
        public void currentTimeEntry_delete(int deletionIndex)
        {
            if (scrollStack.Children.Count > deletionIndex &&
                (scrollStack.Children[deletionIndex] as TimeEntry).entryID == deletionIndex)
                scrollStack.Children.RemoveAt(deletionIndex);
        }
        /// <summary>
        /// Registered to SessionManager.newAddedCommentEvent Event
        /// </summary>
        /// <param name="comment">the string comment</param>
        private void currentTimeEntry_newComment(commentEntry newEntry)
        {
            commentStack.Children.Add(newEntry);
        }
        /// <summary>
        /// Registered to SessionManager.commentEntryDeletedEvent Event
        /// </summary>
        /// <param name="deletionIndex"></param>
        private void entry_delete(int deletionIndex)
        {
            if (commentStack.Children.Count > deletionIndex&&
                (scrollStack.Children[deletionIndex] as commentEntry).entryID == deletionIndex)
                commentStack.Children.RemoveAt(deletionIndex);
        }
        /// <summary>
        /// Registered to MainWindow.entryadderButton.Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void entryAdder_Click(object sender, RoutedEventArgs e)
        {
            TimeEntry newEntry = SM.addNewTimeEntry();
            //Subscribe to the assorted events
            newEntry.textChangedEvent += commentBox_TextChanged;
            scrollStack.Children.Add(newEntry);
        }
        /// <summary>
        /// Registered to MainWindow.intelPopup.intelListBox.SelectionChanged Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void intelListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox temp = sender as ListBox;
            if (temp.SelectedItem != null)
                _currentTextbox.Text = ((commentEntry)temp.SelectedItem).comment;

            temp.SelectedItem = null;
            intelPopup.IsOpen = false;
        }
        /// <summary>
        /// Registered to MainWindow.KeysDown Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="kea"></param>
        private void KeysDown(object sender, KeyEventArgs kea)
        {
            switch (kea.Key)
            {
                case Key.Enter:
                    if (commentAddingBox.IsFocused)
                    {
                        //adds a comment Entry to the comment library
                        if (commentAddingBox.Text != defaultComment && commentAddingBox.Text != "")
                            SM.newCommentEntry(commentAddingBox.Text);
                        this.Focus();
                    }
                    if (_currentTextbox != null && !_currentTextbox.IsFocused)
                    {
                        intelPopup.IsOpen = false;
                    }
                    if (_isSelecting)
                    {

                    }
                    break;
                case Key.Escape:
                    this.Close();
                    break;
                case Key.Space:
                    if (!_isSelecting)
                    {
                        entryAdder_Click(null, null);
                    }
                    break;
                /*
            case Key.E:
                    
                if (!_isSelecting && currentSession.timeEntries.Count != 0)
                {
                    selectionIndex = 0;
                    currentSelectedEntry = currentSession.timeEntries[selectionIndex];
                    currentSelectedEntry.isSelected = true;
                    _isSelecting = true;
                }
                else if (_isSelecting)
                {
                    currentSelectedEntry.isSelected = false;
                    _isSelecting = false;
                }
                break;
                */
                case Key.Up:
                    if (_isSelecting)
                    {
                        if (selectionIndex == 0)
                            selectionIndex = SM.currentSession.timeEntries.Count - 1;
                        else
                            selectionIndex--;
                        currentSelectedEntry.isSelected = false;
                        currentSelectedEntry = SM.currentSession.timeEntries[selectionIndex];
                        currentSelectedEntry.isSelected = true;
                        //currentSelectedEntry.Focus();//this works for forcing scroll
                    }
                    break;
                case Key.Down:
                    if (_isSelecting)
                    {
                        selectionIndex = (selectionIndex + 1) % SM.currentSession.timeEntries.Count;
                        currentSelectedEntry.isSelected = false;
                        currentSelectedEntry = SM.currentSession.timeEntries[selectionIndex];
                        currentSelectedEntry.isSelected = true;
                    }
                    break;
            }
        }
        /// <summary>
        /// Registered to MainWindow.Closed Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            //Serialize the object
            XmlSerializer XMLS = new XmlSerializer(typeof(SessionManager));
            StreamWriter sw = new StreamWriter(SESSION_FILENAME);
            XMLS.Serialize(sw, SM);
            sw.Close();
        }
        #endregion

        #region Class Methods
        private void Deserialize()
        {
            XmlSerializer XMLS = new XmlSerializer(typeof(SessionManager));
            FileStream fs = new FileStream(SESSION_FILENAME, FileMode.Open);
            SM = (SessionManager)XMLS.Deserialize(fs);

        }
        #endregion

        #endregion
    }
}