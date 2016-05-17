using Windows.UI.Xaml.Controls;
using WoChat.ViewModels;

namespace WoChat.Views {
    public sealed partial class ContactsPage : Page {
        private MixedContactViewModel MixedContactVM = App.AppVM.MixedContactVM;

        public ContactsPage() {
            InitializeComponent();
        }


        //private StubViewModel model;
        //private ObservableCollection<friendObserve> friends;
        // Needs to be Show in the Page
        //class friendObserve
        //{
        //    public string name { get; set; }
        //    public string icon { get; set; }
        //    public string id { get; set; }
        //    public string chatid { get; set; }
        //    public friendObserve(string name, string icon , string id , string chatid)
        //    {
        //        this.name = name;
        //        this.icon = icon;
        //        this.id = id;
        //        this.chatid = chatid;
        //    }
        //}

        //private void initFriendObservableCollection()
        //{
        //    ObservableCollection<FriendViewModel> friendList;
        //    if (model.getCurrentUser() == null) return;
        //    // Get the normal friendList
        //    friendList = model.getFriends();
        //    UserModel temp;
        //}

        //public void testDB(object sender, RoutedEventArgs e)
        //{
        //    List<UserModel> myModel =  model.showTestDatabases();
        //    List<string> refr;
        //    //for (int i = 0; i < myModel.Count; i++)
        //    //{
        //        string res = "The User is : \n";
        //        res += "UserName: " + myModel.ElementAt(0).getName() + "\n";
        //        res += "NickName: " + myModel.ElementAt(0).getNick() + "\n";
        //        res += "Email: " + myModel.ElementAt(0).getInfo().email + "\n";
        //        // Show Friends
        //        res += "Friends: ";
        //        refr = myModel.ElementAt(0).getFriends();
        //        for (int ii = 0; ii < refr.Count; ii++)
        //        {
        //            res += refr.ElementAt(ii) + " ";
        //        }
        //        res += "\n";

        //        res += "Groups: ";
        //        refr = myModel.ElementAt(0).getGroups();
        //        for (int ii = 0; ii < refr.Count; ii++)
        //        {
        //            res += refr.ElementAt(ii) + " ";
        //        }
        //        res += "\n";

        //        res += "Chats: ";
        //        refr = myModel.ElementAt(0).getChats();
        //        for (int ii = 0; ii < refr.Count; ii++)
        //        {
        //            res += refr.ElementAt(ii) + " ";
        //        }
        //        res += "\n";

        //        var c = new MessageDialog(res).ShowAsync();
        //    //}
        //}




        //OpenChatLogic:

        //Step 1: Find A friend by id then find the ChatList
        //public ChatViewModel getChatIDByFriendID(string fid)
        //{
        //    return model.getChatByFriend(fid);
        //}

        ////Step 2: open a specified Chat 
        //public List<MessageModel> getMessagesFromChat(ChatModel s)
        //{
        //    return s.getChat();
        //}

        ////Step3 : Show MEssages
        //public void showMessage(List<MessageModel> messages)
        //{
        //    for (int i = 0; i < messages.Count; i++)
        //    {
        //        // Get sender by getSender() method
        //        string sender = messages.ElementAt(i).getSender();
        //        // Get Receiver By getReceiver() method
        //        string receiver = messages.ElementAt(i).getReceiver();
        //        // Get Send/Receive Time By getTime()
        //        string messageTime = messages.ElementAt(i).getTime();
        //        // Get Content by GetContent() Method
        //        string content = messages.ElementAt(i).getContent();
        //        // Get isGroupMessage by getFlag() method
        //        bool isGroup = messages.ElementAt(i).getFlag();


        //        // Now you can implemented a lot things
        //    }
        //}









        //Add Friends Logic:

        //Step 1: Search A friend By UserName(Keyword)
        //public string getUserId(string username)
        //{
        //    string id = model.searchUserByName(username);
        //    if (id == "NOTFOUND")
        //    {
        //        //Handle Execptions
        //    } else
        //    {
        //        // Now id is the User Id
        //    }
        //    return null;
        //}


        //Step 2 : Check if a friend is addable
        //public bool checkAddabilityById(string userid)
        //{
        //    if (model.checkAddability(userid))
        //    {
        //        //do Something if a friend is addable
        //        return true;
        //    } else
        //    {
        //        //Now a friend is Not addable 
        //        // do something to solve it.
        //        return false;
        //    }
        //}

        //Step 3 : Add a friend by given id;
        //public bool addFriendById(string userid)
        //{
        //    if (checkAddabilityById(userid))
        //    {
        //        if (model.addFriend(userid)) {
        //            // Step 4: After adding , we can update our datas to keep synchonized.
        //            // Realized By The StubViewModel.cs
        //            // Here Can do something else
        //            return true;
        //        } else return true;
        //    } else
        //    {
        //        // Now friend is not addable
        //        return false;
        //    }
        //}
    }
}
