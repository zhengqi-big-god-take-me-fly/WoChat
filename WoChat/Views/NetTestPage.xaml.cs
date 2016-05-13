using System;
using System.Collections.Generic;
using System.Diagnostics;
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

using WoChat.Net;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WoChat.Views {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NetTestPage : Page {
        private PostUsersParams Params = new PostUsersParams();
        private PostUsersResult Result = new PostUsersResult();

        public NetTestPage() {
            this.InitializeComponent();
        }

        private async void Submit_Click(object sender, RoutedEventArgs e) {
            Result = await HTTP.PostUsers(Params.username, Params.password);
            Debug.WriteLine(Result.StatusCode);
        }

        private void Log_Click(object sender, RoutedEventArgs e) {
        }
    }
}
