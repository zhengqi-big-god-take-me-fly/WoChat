using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.UI.Core;

namespace WoChat.Net {
    /// <summary>
    /// A whole class to process socket conenctions.
    /// You should only hold on instance.
    /// </summary>
    public class SocketWorker {
        private Timer heartTimer;

        /// <summary>
        /// Call once when App launched.
        /// </summary>
        public void Init(string _hostname, string _port) {
            Hostname = _hostname;
            Port = _port;
            socket = new StreamSocket();
        }

        /// <summary>
        /// Connect to socket server and start listening
        /// </summary>
        public async void Connect() {
            await socket.ConnectAsync(new HostName(Hostname), Port);
#pragma warning disable CS4014 // Need await
            Task.Factory.StartNew(() => ListenForData());
#pragma warning restore CS4014 // Need await
            await Login();
            heartTimer = new Timer(SendHeart, null, 30 * 1000, 30 * 1000);
        }

        public async void ListenForData() {
            string res;
            while (true) {
                res = await ReadData();
                if (res == null) {
                    Disconnect();
                    Connect();
                    break;
                } else if (res.Equals("")) {
                    Disconnect();
                    break;
                } else {
                    Debug.WriteLine("SOCKET>>>>>>>>> " + res);
                    RouteResponse(JObject.Parse(res));
                }
            }
        }

        public async void SendHeart(object state) {
            HeartRequest data = new HeartRequest();
            string req = JsonConvert.SerializeObject(data);
            await WriteData(req);
        }

        public void RouteResponse(JObject res) {
            //TODO
            string type = res["type"].ToString();
            if (type.Equals("msg")) {
                dispatchMessages(JsonConvert.DeserializeObject<List<PushMessage>>(res["data"].ToString()));
            } else if (type.Equals("authrst")) {
                AuthResponse ar = JsonConvert.DeserializeObject<AuthResponse>(res.ToString());
                if (ar.data.code != 0) {
                    Debug.WriteLine("Auth response ERROR");
                    Disconnect();
                    Connect();
                }
            } else if (type.Equals("quit")) {
                Disconnect();
            }
        }

        /// <summary>
        /// Disconnect from server
        /// </summary>
        public void Disconnect() {
            heartTimer.Dispose();
            socket.Dispose();
        }

        public async Task Login() {
            AuthRequest reqObj = new AuthRequest() {
                data = new AuthRequest.Data() {
                    token = App.AppVM.LocalUserVM.JWT
                }
            };
            string req = JsonConvert.SerializeObject(reqObj);
            await WriteData(req);
        }

        public async void MessagesRead(List<string> mids) {
            if (mids.Count == 0) return;
            MessageReceipt mr = new MessageReceipt();
            mr.data = mids;
            string req = JsonConvert.SerializeObject(mr);
            await WriteData(req);
        }

        private async Task WriteData(string data) {
            StreamWriter writer = new StreamWriter(socket.OutputStream.AsStreamForWrite());
            await writer.WriteAsync(data);
            await writer.FlushAsync();
        }

        private async Task<string> ReadData() {
            StreamReader reader = new StreamReader(socket.InputStream.AsStreamForRead());
            char[] ch = new char[65536];
            int i = 0;
            while (true) {
                try {
                    await reader.ReadAsync(ch, i++, 1);
                    if (ch[i - 1] == '\0' || (i > 1 && ch[i - 1] == '\n' && ch[i - 2] == '\n')) break;
                } catch (Exception ex) {
                    // Ungracefully closed
                    Debug.WriteLine(ex.Message);
                    return null;
                }
            }
            if (ch[i - 1] == '\0') {
                // Gracefully closed
                return "";
            } else {
                // Data read
                return new StringBuilder().Append(ch, 0, i - 2).ToString();
            }
        }

        public string Hostname {
            get {
                return hostname;
            }
            set {
                hostname = value;
            }
        }
        public string Port {
            get {
                return port;
            }
            set {
                port = value;
            }
        }
        public CoreDispatcher Dispatcher {
            get {
                return dispatcher;
            }
            set {
                dispatcher = value;
                dispatchMessages(null);
            }
        }

        private async void dispatchMessages(List<PushMessage> pm) {
            if (pm != null) {
                queuedMessages.AddRange(pm);
            }
            if (queuedMessages.Count == 0) return;
            if (Dispatcher != null) {
                // Trigger message event on UI thread
                var self = this;
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    OnMessageArrive(self, new MessageArriveEventArgs(queuedMessages));
                    queuedMessages = new List<PushMessage>();
                });
            }
        }

        private StreamSocket socket = null;
        private string hostname = "";
        private string port = "";
        private CoreDispatcher dispatcher = null;
        private List<PushMessage> queuedMessages = new List<PushMessage>();

        public event MessageArriveEventHandler OnMessageArrive = delegate { };
    }

    public class PushSocketResponse {
        public string type;
        public PushSocketData data;
        public class PushSocketData {

        }
    }

    public class AuthRequest {
        public string type = "auth";
        public Data data;
        public class Data {
            public string token;
        }
    }

    public class HeartRequest {
        public string type = "heart";
    }

    public class AuthResponse {
        public string type;
        public Data data;
        public class Data {
            public int code;
            public string message;
        }
    }
    
    public delegate void MessageArriveEventHandler(object sender, MessageArriveEventArgs e);

    public class MessageArriveEventArgs : EventArgs {
        public List<PushMessage> Messages {
            get {
                return messages;
            }
            private set {
                messages = value;
            }
        }

        public MessageArriveEventArgs(List<PushMessage> pm) {
            Messages = pm;
        }

        private List<PushMessage> messages;
    }
    
    public class PushMessage {
        public string _id;
        public string sender;
        public string receiver;
        public bool to_group;
        public int type;
        public long time;
        public string content;
    }

    public class MessageReceipt {
        public string type = "msgrcpt";
        public List<string> data = new List<string>();
    }
}
