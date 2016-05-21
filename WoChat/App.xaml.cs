using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WoChat.Commons;
using WoChat.Deprecated;
using WoChat.Net;
using WoChat.Views;
using WoChat.ViewModels;
using Windows.UI.Core;

namespace WoChat {
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application {
        // TODO: Remove after refactoring done.
        public static SocketWorker PushSocket = new SocketWorker();
        public static AppViewModel AppVM = new AppViewModel();

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App() {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);
            InitializeComponent();
            Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e) {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached) {
                //DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            // Init socket worker
            PushSocket.Init(PushSocketConfig.Hostname, PushSocketConfig.Port);
            // Chat view model should be able to listen to message arrival
            //PushSocket.OnMessageArrive += AppVM.ChatVM.PushSocket_OnMessageArrive;

            // Change TitleBar color
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView")) {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                if (titleBar != null) {
                    titleBar.BackgroundColor = Current.Resources["SystemAccentColor"] as Color?;
                    titleBar.ButtonBackgroundColor = Current.Resources["SystemAccentColor"] as Color?;
                    titleBar.ButtonForegroundColor = Colors.White;
                    titleBar.ButtonHoverBackgroundColor = Color.FromArgb(255, 221, 161, 85) as Color?;
                }
            }

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null) {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated) {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false) {
                if (rootFrame.Content == null) {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(RegLogPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e) {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e) {
            var deferral = e.SuspendingOperation.GetDeferral();
            PushSocket.Disconnect();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
