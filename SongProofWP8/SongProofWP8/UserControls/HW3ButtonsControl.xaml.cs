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
    public sealed partial class HW3ButtonsControl : UserControl
    {
        private MethodInfo _targetMethod;
        private object _methodTarget;
        public HW3ButtonsControl(string buttonHContent, string buttonWContent, string button3Content,
            string methodName, object methodTarget, Type targetType)
        {
            this.InitializeComponent();
            BH.Content = buttonHContent;
            BW.Content = buttonWContent;
            B3.Content = button3Content;

            _methodTarget = methodTarget;
            _targetMethod = targetType.GetTypeInfo().GetDeclaredMethod(methodName);
        }

        private void IntervalClick(object sender, RoutedEventArgs e)
        {
            _targetMethod.Invoke(_methodTarget, new object[1] { sender });
        }
    }
}
