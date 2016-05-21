using System;
using System.Diagnostics;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using WoChat.Models;
using WoChat.Net;
using WoChat.Utils;
using WoChat.ViewModels;

namespace WoChat.Views {
    public sealed partial class MyProfilePage : Page {
        private LocalUserViewModel LocalUserVM = App.AppVM.LocalUserVM;
        private MyProfilePageUIViewModel UIVM = new MyProfilePageUIViewModel();

        public MyProfilePage() {
            InitializeComponent();
            SystemNavigationManager.GetForCurrentView().BackRequested += MyProfilePage_BackRequested;
        }

        private void MyProfilePage_BackRequested(object sender, BackRequestedEventArgs e) {
            var rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null && rootFrame.CanGoBack && e.Handled == false) {
                rootFrame.GoBack();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            LocalUserVM.Sync();
        }

        private async void UploadAvatar_Click(object sender, RoutedEventArgs e) {
            var picker = new FileOpenPicker();
            picker.CommitButtonText = "上传";
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".bmp");
            picker.FileTypeFilter.Add(".jpg");
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null) {
                PostUsers_AvatarResult result = await HTTP.UploadAvatar(LocalUserVM.JWT, LocalUserVM.LocalUser.Username, file);
                if (result.StatusCode == PostUsers_AvatarResult.PostUsers_AvatarStatusCode.Success) {
                    await LocalUserVM.LocalUser.RemoveAvatar();
                    LocalUserVM.Sync();
                } else {
                    Debug.WriteLine("Upload GG!!!!");
                }
            }
        }

        private void EditNicknameButton_Click(object sender, RoutedEventArgs e) {
            UIVM.IsEditingNickname = true;
        }

        private void AcceptNewNicknameButton_Click(object sender, RoutedEventArgs e) {
            UpdateNewNickname();
        }

        private void CancelNewNicknameButton_Click(object sender, RoutedEventArgs e) {
            UIVM.IsEditingNickname = false;
        }

        private void NewNicknameBox_KeyUp(object sender, KeyRoutedEventArgs e) {
            if (e.Key == Windows.System.VirtualKey.Enter) {
                UpdateNewNickname();
            }
        }

        private async void UpdateNewNickname() {
            UIVM.IsEditingNickname = false;
            LocalUserVM.LocalUser.Nickname = UIVM.EditingNickname;
            PutUsers_Result result = await HTTP.PutUsers_(LocalUserVM.JWT, LocalUserVM.LocalUser.Username, UIVM.EditingNickname, "", "", "", -1, -1);
            switch (result.StatusCode) {
                case PutUsers_Result.PutUser_StatusCode.Success:
                    break;
                default:
                    NotificationHelper.ShowToast("修改失败！");
                    break;
            }
            LocalUserVM.Sync();
        }

        private void MaleRadio_Checked(object sender, RoutedEventArgs e) {
            UpdateNewGender(0);
        }

        private void FemaleRadio_Checked(object sender, RoutedEventArgs e) {
            UpdateNewGender(1);
        }

        private async void UpdateNewGender(int g) {
            if (App.AppVM.LocalUserVM.LocalUser.Gender.Equals(g)) return;
            PutUsers_Result result = await HTTP.PutUsers_(LocalUserVM.JWT, LocalUserVM.LocalUser.Username, "", "", "", "", g, -1);
            switch (result.StatusCode) {
                case PutUsers_Result.PutUser_StatusCode.Success:
                    break;
                default:
                    NotificationHelper.ShowToast("修改失败！");
                    break;
            }
            LocalUserVM.Sync();
        }
    }

    class MyProfilePageUIViewModel : NotifyPropertyChangedBase {
        public bool IsEditingNickname {
            get {
                return isEditingNickname;
            }
            set {
                isEditingNickname = value;
                OnPropertyChanged();
            }
        }

        public string EditingNickname {
            get {
                return editingNickname;
            }

            set {
                editingNickname = value;
                OnPropertyChanged();
            }
        }

        private bool isEditingNickname = false;
        private string editingNickname = "";
    }
}
