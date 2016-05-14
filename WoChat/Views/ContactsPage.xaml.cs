using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WoChat.Models;
using WoChat.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WoChat.Views {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContactsPage : Page {
        private StubViewModel model;
        private ObservableCollection<friendObserve> friends;
        public ContactsPage() {
            this.InitializeComponent();
        }


        // Needs to be Show in the Page
        class friendObserve
        {
            public string name { get; set; }
            public string icon { get; set; }
            public string id { get; set; }
            public string chatid { get; set; }
            public friendObserve(string name, string icon , string id , string chatid)
            {
                this.name = name;
                this.icon = icon;
                this.id = id;
                this.chatid = chatid;
            }
        }

        //private void initFriendObservableCollection()
        //{
        //    ObservableCollection<FriendViewModel> friendList;
        //    if (model.getCurrentUser() == null) return;
        //    // Get the normal friendList
        //    friendList = model.getFriends();
        //    UserModel temp;
        //}






        //OpenChatLogic:

        //Step 1: Find A friend by id then find the ChatList
        public ChatViewModel getChatIDByFriendID(string fid)
        {
            return model.getChatByFriend(fid);
        }

        //Step 2: open a specified Chat 
        public List<MessageModel> getMessagesFromChat(ChatModel s)
        {
            return s.getChat();
        }

        //Step3 : Show MEssages
        public void showMessage(List<MessageModel> messages)
        {
            for (int i = 0; i < messages.Count; i++)
            {
                // Get sender by getSender() method
                string sender = messages.ElementAt(i).getSender();
                // Get Receiver By getReceiver() method
                string receiver = messages.ElementAt(i).getReceiver();
                // Get Send/Receive Time By getTime()
                DateTimeOffset messageTime = messages.ElementAt(i).getTime();
                // Get Content by GetContent() Method
                string content = messages.ElementAt(i).getContent();
                // Get isGroupMessage by getFlag() method
                bool isGroup = messages.ElementAt(i).getFlag();


                // Now you can implemented a lot things
            }
        }









        //Add Friends Logic:

        //Step 1: Search A friend By UserName(Keyword)
        public string getUserId(string username)
        {
            string id = model.searchUserByName(username);
            if (id == "NOTFOUND")
            {
                //Handle Execptions
            } else
            {
                // Now id is the User Id
            }
            return null;
        }


        //Step 2 : Check if a friend is addable
        public bool checkAddabilityById(string userid)
        {
            if (model.checkAddability(userid))
            {
                //do Something if a friend is addable
                return true;
            } else
            {
                //Now a friend is Not addable 
                // do something to solve it.
                return false;
            }
        }

        //Step 3 : Add a friend by given id;
        public bool addFriendById(string userid)
        {
            if (checkAddabilityById(userid))
            {
                if (model.addFriend(userid)) {
                    // Step 4: After adding , we can update our datas to keep synchonized.
                    // Realized By The StubViewModel.cs
                    // Here Can do something else
                    return true;
                } else return true;
            } else
            {
                // Now friend is not addable
                return false;
            }
        }
    }
}
