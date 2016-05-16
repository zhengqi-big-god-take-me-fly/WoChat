using Windows.UI.Xaml.Controls;
using WoChat.ViewModels;

namespace WoChat.Views {
    public sealed partial class ChatPage : Page {
        private ChatViewModel ChatVM = App.AppVM.ChatVM;
        
        public ChatPage() {
            InitializeComponent();
        }

        //private LocalUserViewModel LocalUserVM = App.LocalUserVM;
        //private StubViewModel svm = new StubViewModel();
        //
        //class messageObserve
        //{
        //    public string sender { get; set; }
        //    public string receiver { get; set; }
        //    public string content { get; set; }
        //    public bool isGroup { get; set; }
        //    public DateTimeOffset time { get; set; }
        //    public messageObserve(string sender , string receiver , string content , DateTimeOffset time , bool isGroup = false)
        //    {
        //        this.sender = sender;
        //        this.receiver = receiver;
        //        this.content = content;
        //        this.time = time;
        //        this.isGroup = isGroup;
        //    }
        //}
        //
        //class chatObserve
        //{
        //    public string chater {get;set;}
        //    public string chatee {get;set;}
        //    public string chaterid {get;set;}
        //    public string chateeid {get;set;}
        //    public string chatid {get; set;}
        //    public bool isGroup { get; set; }
        //    public ObservableCollection<messageObserve> messages { get; set; }
        //    public chatObserve(string chater , string chatee , string chaterid , string chateeid ,string chatid , ObservableCollection<messageObserve> messages , bool isGroup = false)
        //    {
        //        this.chater = chater;
        //        this.chatee = chatee;
        //        this.chaterid = chaterid;
        //        this.chateeid = chateeid;
        //        this.messages = messages;
        //        this.isGroup = isGroup;
        //    }
        //
        //}
        //
        //private void initObserveModel(string cid)
        //{
        //    //Models.ChatModel c = model.get
        //}
    }
}
