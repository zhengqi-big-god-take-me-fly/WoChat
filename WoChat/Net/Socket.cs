using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

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
            IsConnected = true;
            Task.Factory.StartNew(() => ListenForData());
            await Login();
            //TODO: Heart package
            heartTimer = new Timer(SendHeart, null, 30 * 1000, 30 * 1000);
        }

        public async void ListenForData() {
            string res;
            while (IsConnected) {
                res = await ReadData();
                //Debug.WriteLine("SOCKET>>>>>>>>> " + res);
                if (res == null || res.Equals("")) continue;
                RouteResponse(JObject.Parse(res));
            }
        }

        public async void SendHeart(object state) {
            //TODO
            HeartRequest data = new HeartRequest();
            string req = JsonConvert.SerializeObject(data);
            await WriteData(req);
        }

        public void RouteResponse(JObject res) {
            //TODO
            string type = res["type"].ToString();
            Debug.WriteLine("TYPE::::" + type);
            if (type.Equals("msg")) {
                OnMessageArrive(this, new MessageArriveEventArgs(JsonConvert.DeserializeObject<List<PushMessage>>(res["data"].ToString())));
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
        public async void Disconnect() {
            await socket.CancelIOAsync();
            socket.Dispose();
            IsConnected = false;
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
            MessageReceipt mr = new MessageReceipt();
            mr.data = mids;
            string req = JsonConvert.SerializeObject(mr);
            await WriteData(req);
        }

        public async Task WriteData(string data) {
            StreamWriter writer = new StreamWriter(socket.OutputStream.AsStreamForWrite());
            await writer.WriteAsync(data);
            await writer.FlushAsync();
        }

        public async Task<string> ReadData() {
            StreamReader reader = new StreamReader(socket.InputStream.AsStreamForRead());
            char[] ch = new char[65536];
            int i = 0;
            while (IsConnected) {
                try {
                    await reader.ReadAsync(ch, i++, 1);
                    if (i > 1 && ch[i - 1] == '\n' && ch[i - 2] == '\n') break;
                } catch (Exception ex) {
                    Debug.WriteLine("Socket closed unexpectedly!");
                    Disconnect();
                    Connect();
                }
            }
            return new StringBuilder().Append(ch, 0, i - 2).ToString();
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
        public bool IsConnected {
            get {
                return isConnected;
            }
            set {
                isConnected = value;
            }
        }

        private StreamSocket socket = null;
        private string hostname = "";
        private string port = "";
        private bool isConnected = false;

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
        public MessageArriveEventArgs(List<PushMessage> pm) {
            message = pm;
        }

        private List<PushMessage> message;
    }
    
    public class PushMessage {
        public string _id;
        public string sender;
        public string receiver;
        public string to_group;
        public int type;
        public long time;
        public string content;
    }

    public class MessageReceipt {
        public string type = "msgrcpt";
        public List<string> data = new List<string>();
    }
}
