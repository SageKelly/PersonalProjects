using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public sealed partial class SessionButtonsControl : UserControl
    {
        private bool _navigationSetup = false;

        MethodInfo StartMethodInvoker;
        MethodInfo QuitMethodInvoker;
        MethodInfo ViewResultMethodInvoker;

        private object _methodTarget;

        public SessionButtonsControl(string startMethodName, string quitMethodName, string viewResultMethodName,
            object methodTarget, Type targetType)
        {
            this.InitializeComponent();

            _methodTarget = methodTarget;

            StartMethodInvoker = targetType.GetTypeInfo().GetDeclaredMethod(startMethodName);
            QuitMethodInvoker = targetType.GetTypeInfo().GetDeclaredMethod(quitMethodName);
            ViewResultMethodInvoker = targetType.GetTypeInfo().GetDeclaredMethod(viewResultMethodName);


        }

        #region Extra Button Handlers
        private void B_ViewResults_Click(object sender, RoutedEventArgs e)
        {
            ViewResultMethodInvoker.Invoke(_methodTarget, new object[1] { sender });
        }

        private void B_Start_Click(object sender, RoutedEventArgs e)
        {
            StartMethodInvoker.Invoke(_methodTarget, new object[1] { sender });

        }

        private void B_Quit_Click(object sender, RoutedEventArgs e)
        {
            QuitMethodInvoker.Invoke(_methodTarget, new object[1] { sender });
        }

        public void EnableViewResults(bool isEnabled = true)
        {
            B_ViewResults.IsEnabled = isEnabled;
        }

        public void EnableStart(bool isEnabled = true)
        {
            B_Start.IsEnabled = isEnabled;
        }
        #endregion
    }
}
