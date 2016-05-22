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


// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SongProofWP8.UserControls
{
    public sealed partial class PianoKeyControl : UserControl
    {
        private MethodInfo _noteClickMethod;
        private object _methodTarget;

        public PianoKeyControl(string[] sessionPiano, string[] usedNotes,
            string pianoKeysMethodName, object methodTarget, Type targetType)
        {
            this.InitializeComponent();
            DataContext = this;
            IC_Buttons.ItemsSource = sessionPiano;
            _noteClickMethod = targetType.GetTypeInfo().GetDeclaredMethod(pianoKeysMethodName);
            _methodTarget = methodTarget;

            LBScale.ItemsSource = usedNotes;
            IC_Buttons.ItemsSource = sessionPiano;
        }

        private void _noteClick(object sender, RoutedEventArgs e)
        {
            _noteClickMethod.Invoke(_methodTarget, new object[1] { sender });
        }

        private void ToggleScaleView(object sender, RoutedEventArgs e)
        {
            if ((bool)B_Cheat.IsChecked)
            {
                B_Cheat.Content = "Be Honest?";
                FadeIn.Begin();
            }
            else
            {
                B_Cheat.Content = "Cheat?";
                FadeOut.Begin();
            }
        }
    }
}
