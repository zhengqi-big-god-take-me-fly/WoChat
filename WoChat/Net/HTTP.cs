using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WoChat.Utils;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Windows.Storage;
using System.Text;

namespace WoChat.Net {
    class HTTP {
        // Host
        public static string API_HOST = "http://tidyzq.com:3000";

        // Uri
        public static string URI_USERS = "/users";
        public static string URI_USERS_ = "/users/";
        public static string URI_AUTH_LOGIN = "/auth/login";
        public static string URI_CONTACTS = "/contacts";
        public static string URI_CONTACTS_ = "/contacts/";
        public static string URI_INVITATION = "/invitation";
        public static string URI_MESSAGE = "/message";
        public static string URI_AVATAR = "/avatar";

        public static async Task<GetChatGroup_Result> GetChatGroup_(string jwt, string gi) {
            GetChatGroup_Result result;
            if (jwt.Equals("") || gi.Equals("")) {
                return new GetChatGroup_Result() { StatusCode = GetChatGroup_Result.GetChatGroup_StatusCode.UnknownError };
            }
            HttpClient client = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage() {
                Method = HttpMethod.Get,
                RequestUri = new Uri(API_HOST + "/chat_group/" + gi)
            };
            req.Headers["Authorization"] = jwt;
            HttpResponseMessage res = await client.SendRequestAsync(req);
            try {
                result = JsonConvert.DeserializeObject<GetChatGroup_Result>(res.Content.ToString());
                switch (res.StatusCode) {
                    case HttpStatusCode.Ok:
                        result.StatusCode = GetChatGroup_Result.GetChatGroup_StatusCode.Success;
                        break;

                    case HttpStatusCode.Unauthorized:
                        result.StatusCode = GetChatGroup_Result.GetChatGroup_StatusCode.InvalidToken;
                        break;
                    case HttpStatusCode.NotFound:
                        result.StatusCode = GetChatGroup_Result.GetChatGroup_StatusCode.NoThisGroup;
                        break;
                    default:
                        result.StatusCode = GetChatGroup_Result.GetChatGroup_StatusCode.UnknownError;
                        break;
                }
#pragma warning disable CS0168 // Variable is declared but never used
            } catch (JsonException e) {
#pragma warning restore CS0168 // Variable is declared but never used
                result = new GetChatGroup_Result() { StatusCode = GetChatGroup_Result.GetChatGroup_StatusCode.UnknownError };
            }
            return result;
        }

        public static async Task<PostUsers_AvatarResult> UploadAvatar(string jwt, string un, StorageFile file) {
            if (file == null || jwt.Equals("") || un.Equals("")) {
                return new PostUsers_AvatarResult() { StatusCode = PostUsers_AvatarResult.PostUsers_AvatarStatusCode.UnknownError };
            }
            PostUsers_AvatarResult result = new PostUsers_AvatarResult();
            HttpClient client = new HttpClient();
            // Process Content for this post request.
            HttpMultipartFormDataContent content = new HttpMultipartFormDataContent();
            // Read file content to a HttpStreamContent which will be added into post request content.
            Stream fileStream = await file.OpenStreamForReadAsync();
            HttpStreamContent avatarContent = new HttpStreamContent(fileStream.AsInputStream());
            avatarContent.Headers["Content-Type"] = file.ContentType;
            content.Add(avatarContent, "avatar", file.Name);
            HttpRequestMessage req = new HttpRequestMessage() {
                RequestUri = new Uri(API_HOST + URI_USERS_ + un + URI_AVATAR),
                Method = HttpMethod.Post,
                Content = content
            };
            req.Headers["Authorization"] = jwt;
            HttpResponseMessage res = await client.SendRequestAsync(req);
            switch (res.StatusCode) {
                case HttpStatusCode.Created:
                    result.StatusCode = PostUsers_AvatarResult.PostUsers_AvatarStatusCode.Success;
                    break;
                case HttpStatusCode.BadRequest:
                    result.StatusCode = PostUsers_AvatarResult.PostUsers_AvatarStatusCode.InvalidContent;
                    break;
                case HttpStatusCode.Unauthorized:
                    result.StatusCode = PostUsers_AvatarResult.PostUsers_AvatarStatusCode.InvalidToken;
                    break;
                case HttpStatusCode.NotFound:
                    result.StatusCode = PostUsers_AvatarResult.PostUsers_AvatarStatusCode.NoThisUser;
                    break;
                default:
                    result.StatusCode = PostUsers_AvatarResult.PostUsers_AvatarStatusCode.UnknownError;
                    break;
            }
            return result;
        }

        public static async Task<GetChatGrouup_MembersResult> GetChatGroup_Members(string jwt, string gi) {

            {
                GetChatGrouup_MembersResult result;
                if (jwt.Equals("") || gi.Equals("")) {
                    return new GetChatGrouup_MembersResult() { StatusCode = GetChatGrouup_MembersResult.GetChatGrouup_MembersStatusCode.UnknownError };
                }
                HttpClient client = new HttpClient();
                HttpRequestMessage req = new HttpRequestMessage() {

                    Method = HttpMethod.Get,
                    RequestUri = new Uri(API_HOST + "/chat_group/" + gi + "/members")
                };
                req.Headers["Authorization"] = jwt;
                HttpResponseMessage res = await client.SendRequestAsync(req);
                try {
                    result = JsonConvert.DeserializeObject<GetChatGrouup_MembersResult>(res.Content.ToString());
                    switch (res.StatusCode) {
                        case HttpStatusCode.Ok:
                            result.StatusCode = GetChatGrouup_MembersResult.GetChatGrouup_MembersStatusCode.Success;
                            break;

                        case HttpStatusCode.Unauthorized:
                            result.StatusCode = GetChatGrouup_MembersResult.GetChatGrouup_MembersStatusCode.InvalidToken;
                            break;
                        case HttpStatusCode.NotFound:
                            result.StatusCode = GetChatGrouup_MembersResult.GetChatGrouup_MembersStatusCode.NoThisGroup;
                            break;
                        default:
                            result.StatusCode = GetChatGrouup_MembersResult.GetChatGrouup_MembersStatusCode.UnknownError;
                            break;
                    }
#pragma warning disable CS0168 // Variable is declared but never used
                } catch (JsonException e) {
#pragma warning restore CS0168 // Variable is declared but never used
                    result = new GetChatGrouup_MembersResult() { StatusCode = GetChatGrouup_MembersResult.GetChatGrouup_MembersStatusCode.UnknownError };
                }
                return result;
            }

        }

        public static async Task<GetUsers_Result> GetUsers_(string un) {
            GetUsers_Result result = new GetUsers_Result();
            if (un.Equals("")) {
                return new GetUsers_Result() { StatusCode = GetUsers_Result.GetUsers_StatusCode.UnknownError };
            }
            HttpClient client = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage() {
                Method = HttpMethod.Get,
                RequestUri = new Uri(API_HOST + URI_USERS_ + un)
            };
            HttpResponseMessage res = await client.SendRequestAsync(req);
            try {
                switch (res.StatusCode) {
                    case HttpStatusCode.Ok:
                        result = JsonConvert.DeserializeObject<GetUsers_Result>(res.Content.ToString());
                        result.StatusCode = GetUsers_Result.GetUsers_StatusCode.Success;
                        break;
                    case HttpStatusCode.NotFound:
                        result.StatusCode = GetUsers_Result.GetUsers_StatusCode.NoThisUser;
                        break;
                    default:
                        result.StatusCode = GetUsers_Result.GetUsers_StatusCode.UnknownError;
                        break;
                }
#pragma warning disable CS0168 // Variable is declared but never used
            } catch (JsonException e) {
#pragma warning restore CS0168 // Variable is declared but never used
                return new GetUsers_Result() { StatusCode = GetUsers_Result.GetUsers_StatusCode.UnknownError };
            }
            return result;
        }

        // Test pass in success situation
        public static async Task<GetUsers_ContactsResult> GetUsers_Contacts(string jwt, string un) {
            GetUsers_ContactsResult result;
            if (jwt.Equals("") || un.Equals("")) {
                return new GetUsers_ContactsResult() { StatusCode = GetUsers_ContactsResult.GetUsers0ContactsStatusCode.UnknownError };
            }
            HttpClient client = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage() {
                Method = HttpMethod.Get,
                RequestUri = new Uri(API_HOST + URI_USERS_ + un + URI_CONTACTS)
            };
            req.Headers["Authorization"] = jwt;
            HttpResponseMessage res = await client.SendRequestAsync(req);
            try {
                result = JsonConvert.DeserializeObject<GetUsers_ContactsResult>(res.Content.ToString());
                switch (res.StatusCode) {
                    case HttpStatusCode.Ok:
                        result.StatusCode = GetUsers_ContactsResult.GetUsers0ContactsStatusCode.Success;
                        break;
                    case HttpStatusCode.Unauthorized:
                        result.StatusCode = GetUsers_ContactsResult.GetUsers0ContactsStatusCode.InvalidToken;
                        break;
                    case HttpStatusCode.NotFound:
                        result.StatusCode = GetUsers_ContactsResult.GetUsers0ContactsStatusCode.NoThisUser;
                        break;
                    default:
                        result.StatusCode = GetUsers_ContactsResult.GetUsers0ContactsStatusCode.UnknownError;
                        break;
                }
#pragma warning disable CS0168 // Variable is declared but never used
            } catch (JsonException e) {
#pragma warning restore CS0168 // Variable is declared but never used
                result = new GetUsers_ContactsResult() { StatusCode = GetUsers_ContactsResult.GetUsers0ContactsStatusCode.UnknownError };
            }
            return result;
        }

        // Test pass in success situation
        public static async Task<PostAuthLoginResult> PostAuthLogin(string un, string pw) {
            PostAuthLoginResult result = new PostAuthLoginResult();
            if (un.Equals("") || pw.Equals("")) {
                return new PostAuthLoginResult() { StatusCode = PostAuthLoginResult.PostAuthLoginStatusCode.UnknownError };
            }
            HttpClient client = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage() {
                Content = new HttpStringContent(JsonConvert.SerializeObject(new PostAuthLoginParams() {
                    username = un,
                    password = MD5.Hash(pw)
                }), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"),
                Method = HttpMethod.Post,
                RequestUri = new Uri(API_HOST + URI_AUTH_LOGIN)
            };
            HttpResponseMessage res = await client.SendRequestAsync(req);
            try {
                switch (res.StatusCode) {
                    case HttpStatusCode.Ok:
                        result = JsonConvert.DeserializeObject<PostAuthLoginResult>(res.Content.ToString());
                        result.StatusCode = PostAuthLoginResult.PostAuthLoginStatusCode.Success;
                        break;
                    case HttpStatusCode.Unauthorized:
                        result.StatusCode = PostAuthLoginResult.PostAuthLoginStatusCode.Failure;
                        break;
                    default:
                        result.StatusCode = PostAuthLoginResult.PostAuthLoginStatusCode.Failure;
                        break;
                }
#pragma warning disable CS0168 // Variable is declared but never used
            } catch (JsonException e) {
#pragma warning restore CS0168 // Variable is declared but never used
                result = new PostAuthLoginResult() { StatusCode = PostAuthLoginResult.PostAuthLoginStatusCode.UnknownError };
            }
            return result;
        }

        public static async Task<PostChatGroupResult> PostChatGroup(string jwt, string gn) {
            PostChatGroupResult result;
            if (jwt.Equals("") || gn.Equals("")) {
                return new PostChatGroupResult() { StatusCode = PostChatGroupResult.PostChatGroupStatusCode.UnknownError };
            }
            HttpClient client = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage() {
                Content = new HttpStringContent(JsonConvert.SerializeObject(new PostChatGroupParams() {
                    groupname = gn
                })),
                Method = HttpMethod.Post,
                RequestUri = new Uri(API_HOST + "/chat_group")
            };
            req.Headers["Authorization"] = jwt;
            HttpResponseMessage res = await client.SendRequestAsync(req);
            try {
                result = JsonConvert.DeserializeObject<PostChatGroupResult>(res.Content.ToString());
                switch (res.StatusCode) {
                    case HttpStatusCode.Created:
                        result.StatusCode = PostChatGroupResult.PostChatGroupStatusCode.Success;
                        break;
                    case HttpStatusCode.BadRequest:
                        result.StatusCode = PostChatGroupResult.PostChatGroupStatusCode.InvalidParams;
                        break;
                    case HttpStatusCode.Unauthorized:
                        result.StatusCode = PostChatGroupResult.PostChatGroupStatusCode.InvalidToken;
                        break;

                    default:
                        result.StatusCode = PostChatGroupResult.PostChatGroupStatusCode.UnknownError;
                        break;
                }
#pragma warning disable CS0168 // Variable is declared but never used
            } catch (JsonException e) {
#pragma warning restore CS0168 // Variable is declared but never used
                result = new PostChatGroupResult() { StatusCode = PostChatGroupResult.PostChatGroupStatusCode.UnknownError };
            }
            return result;
        }

        public static async Task<PostChatGroup_MembersResult> PostChatGroup_Members(string jwt, string gi, List<string> ms) {
            PostChatGroup_MembersResult result;
            if (jwt.Equals("") || gi.Equals("") || ms == null) {
                return new PostChatGroup_MembersResult() { StatusCode = PostChatGroup_MembersResult.PostChatGroup_MembersStatusCode.UnknownError };
            }
            HttpClient client = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage() {
                Content = new HttpStringContent(JsonConvert.SerializeObject(new PostChatGroup_MembersParams() {
                    members = ms
                })),
                Method = HttpMethod.Post,
                RequestUri = new Uri(API_HOST + "/chat_group/" + gi + "/members")
            };
            req.Headers["Authorization"] = jwt;
            HttpResponseMessage res = await client.SendRequestAsync(req);
            try {
                result = JsonConvert.DeserializeObject<PostChatGroup_MembersResult>(res.Content.ToString());
                switch (res.StatusCode) {
                    case HttpStatusCode.Created:
                        result.StatusCode = PostChatGroup_MembersResult.PostChatGroup_MembersStatusCode.Success;
                        break;
                    case HttpStatusCode.BadRequest:
                        result.StatusCode = PostChatGroup_MembersResult.PostChatGroup_MembersStatusCode.InvalidParams;
                        break;
                    case HttpStatusCode.Unauthorized:
                        result.StatusCode = PostChatGroup_MembersResult.PostChatGroup_MembersStatusCode.InvalidToken;
                        break;
                    case HttpStatusCode.NotFound:
                        result.StatusCode = PostChatGroup_MembersResult.PostChatGroup_MembersStatusCode.NoThisGroup;
                        break;
                    default:
                        result.StatusCode = PostChatGroup_MembersResult.PostChatGroup_MembersStatusCode.UnknownError;
                        break;
                }
#pragma warning disable CS0168 // Variable is declared but never used
            } catch (JsonException e) {
#pragma warning restore CS0168 // Variable is declared but never used
                result = new PostChatGroup_MembersResult() { StatusCode = PostChatGroup_MembersResult.PostChatGroup_MembersStatusCode.UnknownError };
            }
            return result;
        }

        public static async Task<PostChatGroup_MessagesResult> PostChatGroup_Messages(string jwt, string gi, int tp, string ct) {
            PostChatGroup_MessagesResult result;
            if (jwt.Equals("") || gi.Equals("") || ct.Equals("")) {
                return new PostChatGroup_MessagesResult() { StatusCode = PostChatGroup_MessagesResult.PostChatGroup_MessagesStatusCode.UnknownError };
            }
            HttpClient client = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage() {
                Content = new HttpStringContent(JsonConvert.SerializeObject(new PostChatGroup_MessagesParams() {
                    type = tp,
                    content = ct
                })),
                Method = HttpMethod.Post,
                RequestUri = new Uri(API_HOST + "/chat_group/" + gi + "/messages")
            };
            req.Headers["Authorization"] = jwt;
            HttpResponseMessage res = await client.SendRequestAsync(req);
            try {
                result = JsonConvert.DeserializeObject<PostChatGroup_MessagesResult>(res.Content.ToString());
                switch (res.StatusCode) {
                    case HttpStatusCode.Created:
                        result.StatusCode = PostChatGroup_MessagesResult.PostChatGroup_MessagesStatusCode.Success;
                        break;
                    case HttpStatusCode.BadRequest:
                        result.StatusCode = PostChatGroup_MessagesResult.PostChatGroup_MessagesStatusCode.InvalidParams;
                        break;
                    case HttpStatusCode.Unauthorized:
                        result.StatusCode = PostChatGroup_MessagesResult.PostChatGroup_MessagesStatusCode.InvalidToken;
                        break;
                    case HttpStatusCode.NotFound:
                        result.StatusCode = PostChatGroup_MessagesResult.PostChatGroup_MessagesStatusCode.NoThisGroup;
                        break;
                    default:
                        result.StatusCode = PostChatGroup_MessagesResult.PostChatGroup_MessagesStatusCode.UnknownError;
                        break;
                }
#pragma warning disable CS0168 // Variable is declared but never used
            } catch (JsonException e) {
#pragma warning restore CS0168 // Variable is declared but never used
                result = new PostChatGroup_MessagesResult() { StatusCode = PostChatGroup_MessagesResult.PostChatGroup_MessagesStatusCode.UnknownError };
            }
            return result;
        }

        // Test pass in success situation
        public static async Task<PostUsersResult> PostUsers(string un, string pw) {
            PostUsersResult result = new PostUsersResult();
            if (un.Equals("") || pw.Equals("")) {
                return new PostUsersResult() { StatusCode = PostUsersResult.PostUsersStatusCode.InvalidParams };
            }
            HttpClient client = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage() {
                Content = new HttpStringContent(JsonConvert.SerializeObject(new PostUsersParams() {
                    username = un,
                    password = MD5.Hash(pw)
                }), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"),
                Method = HttpMethod.Post,
                RequestUri = new Uri(API_HOST + URI_USERS)
            };
            HttpResponseMessage res = await client.SendRequestAsync(req);
            switch (res.StatusCode) {
                case HttpStatusCode.Created:
                    result.StatusCode = PostUsersResult.PostUsersStatusCode.Success;
                    break;
                case HttpStatusCode.BadRequest:
                    result.StatusCode = PostUsersResult.PostUsersStatusCode.InvalidParams;
                    break;
                case HttpStatusCode.Conflict:
                    result.StatusCode = PostUsersResult.PostUsersStatusCode.ExistingUser;
                    break;
                default:
                    result.StatusCode = PostUsersResult.PostUsersStatusCode.UnknownError;
                    break;
            }
            return result;
        }

        public static async Task<PostUsers_InvitationResult> PostUsers_Invitation(string jwt, string cn, string m) {
            PostUsers_InvitationResult result;
            if (jwt.Equals("") || cn.Equals("")) {
                return new PostUsers_InvitationResult() { StatusCode = PostUsers_InvitationResult.PostUsers_InvitationStatusCode.UnknownError };
            }
            HttpClient client = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage() {
                Content = new HttpStringContent(JsonConvert.SerializeObject(new PostUsers_InvitationParams() {
                    message = m
                }), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"),
                Method = HttpMethod.Post,
                RequestUri = new Uri(API_HOST + URI_USERS_ + cn + URI_INVITATION)
            };
            req.Headers["Authorization"] = jwt;
            HttpResponseMessage res = await client.SendRequestAsync(req);
            result = new PostUsers_InvitationResult();
            switch (res.StatusCode) {
                case HttpStatusCode.Created:
                    result.StatusCode = PostUsers_InvitationResult.PostUsers_InvitationStatusCode.Success;
                    break;
                case HttpStatusCode.Unauthorized:
                    result.StatusCode = PostUsers_InvitationResult.PostUsers_InvitationStatusCode.InvalidToken;
                    break;
                case HttpStatusCode.NotFound:
                    result.StatusCode = PostUsers_InvitationResult.PostUsers_InvitationStatusCode.NoThisUser;
                    break;
                case HttpStatusCode.Conflict:
                    result.StatusCode = PostUsers_InvitationResult.PostUsers_InvitationStatusCode.AlreadyContact;
                    break;
                default:
                    result.StatusCode = PostUsers_InvitationResult.PostUsers_InvitationStatusCode.UnknownError;
                    break;
            }
            return result;
        }

        public static async Task<PostUsers_MessageResult> PostUsers_Message(string jwt, string cn, int t, string c) {
            PostUsers_MessageResult result;
            if (jwt.Equals("") || cn.Equals("") || c.Equals("")) {
                return new PostUsers_MessageResult() { StatusCode = PostUsers_MessageResult.PostUsers_MessageStatusCode.UnknownError };
            }
            HttpClient client = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage() {
                Content = new HttpStringContent(JsonConvert.SerializeObject(new PostUsers_MessageParams() {
                    type = t,
                    content = c
                }), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"),
                Method = HttpMethod.Post,
                RequestUri = new Uri(API_HOST + URI_USERS_ + cn + URI_MESSAGE)
            };
            req.Headers["Authorization"] = jwt;
            HttpResponseMessage res = await client.SendRequestAsync(req);
            try {
                result = JsonConvert.DeserializeObject<PostUsers_MessageResult>(res.Content.ToString());
                switch (res.StatusCode) {
                    case HttpStatusCode.Created:
                        result.StatusCode = PostUsers_MessageResult.PostUsers_MessageStatusCode.Success;
                        break;
                    case HttpStatusCode.BadRequest:
                        result.StatusCode = PostUsers_MessageResult.PostUsers_MessageStatusCode.InvalidParams;
                        break;
                    case HttpStatusCode.Unauthorized:
                        result.StatusCode = PostUsers_MessageResult.PostUsers_MessageStatusCode.InvalidToken;
                        break;
                    case HttpStatusCode.NotFound:
                        result.StatusCode = PostUsers_MessageResult.PostUsers_MessageStatusCode.NoThisUser;
                        break;
                    default:
                        result.StatusCode = PostUsers_MessageResult.PostUsers_MessageStatusCode.UnknownError;
                        break;
                }
#pragma warning disable CS0168 // Variable is declared but never used
            } catch (JsonException e) {
#pragma warning restore CS0168 // Variable is declared but never used
                result = new PostUsers_MessageResult() { StatusCode = PostUsers_MessageResult.PostUsers_MessageStatusCode.UnknownError };
            }
            return result;
        }

        public static async Task<PutChatGroup_Result> PutChatGroup_(string jwt, string gi, string gn) {
            PutChatGroup_Result result;
            if (jwt.Equals("") || gi.Equals("") || gn.Equals("")) {
                return new PutChatGroup_Result() { StatusCode = PutChatGroup_Result.PutChatGroup_StatusCode.UnknownError };
            }
            HttpClient client = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage() {
                Content = new HttpStringContent(JsonConvert.SerializeObject(new PutChatGroup_Params() {
                    groupname = gn
                })),
                Method = HttpMethod.Put,
                RequestUri = new Uri(API_HOST + "/chat_group/" + gi)
            };
            req.Headers["Authorization"] = jwt;
            HttpResponseMessage res = await client.SendRequestAsync(req);
            try {
                result = JsonConvert.DeserializeObject<PutChatGroup_Result>(res.Content.ToString());
                switch (res.StatusCode) {
                    case HttpStatusCode.Ok:
                        result.StatusCode = PutChatGroup_Result.PutChatGroup_StatusCode.Success;
                        break;
                    case HttpStatusCode.BadRequest:
                        result.StatusCode = PutChatGroup_Result.PutChatGroup_StatusCode.InvalidParams;
                        break;
                    case HttpStatusCode.Unauthorized:
                        result.StatusCode = PutChatGroup_Result.PutChatGroup_StatusCode.InvalidToken;
                        break;
                    case HttpStatusCode.NotFound:
                        result.StatusCode = PutChatGroup_Result.PutChatGroup_StatusCode.NoThisGroup;
                        break;
                    default:
                        result.StatusCode = PutChatGroup_Result.PutChatGroup_StatusCode.UnknownError;
                        break;
                }
#pragma warning disable CS0168 // Variable is declared but never used
            } catch (JsonException e) {
#pragma warning restore CS0168 // Variable is declared but never used
                result = new PutChatGroup_Result() { StatusCode = PutChatGroup_Result.PutChatGroup_StatusCode.UnknownError };
            }
            return result;
        }

        public static async Task<PutChatGroup_Members_Result> PutChatGroup_Members_(string jwt, string gi, string mn, string gn) {
            PutChatGroup_Members_Result result;
            if (jwt.Equals("") || gi.Equals("") || mn.Equals("") || gn.Equals("")) {
                return new PutChatGroup_Members_Result() { StatusCode = PutChatGroup_Members_Result.PutChatGroup_Members_StatusCode.UnknownError };
            }
            HttpClient client = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage() {
                Content = new HttpStringContent(JsonConvert.SerializeObject(new PutChatGroup_Members_Params() {
                    group_nick = gn
                })),
                Method = HttpMethod.Put,
                RequestUri = new Uri(API_HOST + "/chat_group/" + gi + "/members/" + mn)
            };
            req.Headers["Authorization"] = jwt;
            HttpResponseMessage res = await client.SendRequestAsync(req);
            try {
                result = JsonConvert.DeserializeObject<PutChatGroup_Members_Result>(res.Content.ToString());
                switch (res.StatusCode) {
                    case HttpStatusCode.Created:
                        result.StatusCode = PutChatGroup_Members_Result.PutChatGroup_Members_StatusCode.Success;
                        break;
                    case HttpStatusCode.BadRequest:
                        result.StatusCode = PutChatGroup_Members_Result.PutChatGroup_Members_StatusCode.InvalidParams;
                        break;
                    case HttpStatusCode.Unauthorized:
                        result.StatusCode = PutChatGroup_Members_Result.PutChatGroup_Members_StatusCode.InvalidToken;
                        break;
                    case HttpStatusCode.NotFound:
                        result.StatusCode = PutChatGroup_Members_Result.PutChatGroup_Members_StatusCode.NoThisGroup;
                        break;
                    default:
                        result.StatusCode = PutChatGroup_Members_Result.PutChatGroup_Members_StatusCode.UnknownError;
                        break;
                }
#pragma warning disable CS0168 // Variable is declared but never used
            } catch (JsonException e) {
#pragma warning restore CS0168 // Variable is declared but never used
                result = new PutChatGroup_Members_Result() { StatusCode = PutChatGroup_Members_Result.PutChatGroup_Members_StatusCode.UnknownError };
            }
            return result;
        }

        public static async Task<PutUsers_Result> PutUsers_(string jwt, string un, string nn, string pw, string opw, string at, int gd, int rg) {
            PutUsers_Result result;
            if (jwt.Equals("") || un.Equals("") || (nn.Equals("") && pw.Equals("") && opw.Equals("") && at.Equals("") && gd == -1 && rg == -1)) {
                return new PutUsers_Result() { StatusCode = PutUsers_Result.PutUser_StatusCode.UnknownError };
            }
            HttpClient client = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage() {
                Content = new HttpStringContent(PutUsers_GetParams(nn, pw, opw, at, gd, rg)),
                Method = HttpMethod.Put,
                RequestUri = new Uri(API_HOST + "/users/" + un)
            };
            req.Headers["Authorization"] = jwt;
            HttpResponseMessage res = await client.SendRequestAsync(req);
            try {
                result = JsonConvert.DeserializeObject<PutUsers_Result>(res.Content.ToString());
                switch (res.StatusCode) {
                    case HttpStatusCode.Ok:
                        result.StatusCode = PutUsers_Result.PutUser_StatusCode.Success;
                        break;
                    case HttpStatusCode.BadRequest:
                        result.StatusCode = PutUsers_Result.PutUser_StatusCode.InvalidParams;
                        break;
                    case HttpStatusCode.Unauthorized:
                        result.StatusCode = PutUsers_Result.PutUser_StatusCode.InvalidToken;
                        break;
                    case HttpStatusCode.NotFound:
                        result.StatusCode = PutUsers_Result.PutUser_StatusCode.NoThisGroup;
                        break;
                    default:
                        result.StatusCode = PutUsers_Result.PutUser_StatusCode.UnknownError;
                        break;
                }
#pragma warning disable CS0168 // Variable is declared but never used
            } catch (JsonException e) {
#pragma warning restore CS0168 // Variable is declared but never used
                result = new PutUsers_Result() { StatusCode = PutUsers_Result.PutUser_StatusCode.UnknownError };
            }
            return result;
        }

        static string PutUsers_GetParams(string nn, string pw, string opw,  string at, int gd, int rg) {
            StringBuilder sb = new StringBuilder();
            StringWriter jw = new StringWriter(sb);
            using (JsonWriter writer = new JsonTextWriter(jw)) {
                writer.WriteStartObject();
                if (!nn.Equals("")) {
                    writer.WritePropertyName("nickname");
                    writer.WriteValue(nn);
                }
                if (!pw.Equals("") && !opw.Equals("")) {
                    writer.WritePropertyName("password");
                    writer.WriteValue(pw);
                    writer.WritePropertyName("old_password");
                    writer.WriteValue(opw);
                }
                if (!at.Equals("")) {
                    writer.WritePropertyName("avatar");
                    writer.WriteValue(at);
                }
                if (gd != -1) {
                    writer.WritePropertyName("gender");
                    writer.WriteValue(gd);
                }
                if (rg != -1) {
                    writer.WritePropertyName("region");
                    writer.WriteValue(rg);
                }
                writer.WriteEndObject();
            }
            return sb.ToString();
        }

        public static async Task<PutUsers_Contacts_Result> PutUsers_Contacts_(string jwt, string un, string cn, string r, int bl) {
            PutUsers_Contacts_Result result;
            if (jwt.Equals("") || un.Equals("") || cn.Equals("") || r.Equals("")) {
                return new PutUsers_Contacts_Result() { StatusCode = PutUsers_Contacts_Result.PutUsers_Contacts_StatusCode.UnknownError };
            }
            HttpClient client = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage() {
                Content = new HttpStringContent(JsonConvert.SerializeObject(new PutUsers_Contacts_Params() {
                    remark = r,
                    block_level = bl
                })),
                Method = HttpMethod.Put,
                RequestUri = new Uri(API_HOST + URI_USERS_ + un + URI_CONTACTS_ + cn)
            };
            req.Headers["Authorization"] = jwt;
            HttpResponseMessage res = await client.SendRequestAsync(req);
            try {
                result = JsonConvert.DeserializeObject<PutUsers_Contacts_Result>(res.Content.ToString());
                switch (res.StatusCode) {
                    case HttpStatusCode.Ok:
                        result.StatusCode = PutUsers_Contacts_Result.PutUsers_Contacts_StatusCode.Success;
                        break;
                    case HttpStatusCode.BadRequest:
                        result.StatusCode = PutUsers_Contacts_Result.PutUsers_Contacts_StatusCode.InvalidParams;
                        break;
                    case HttpStatusCode.Unauthorized:
                        result.StatusCode = PutUsers_Contacts_Result.PutUsers_Contacts_StatusCode.InvalidToken;
                        break;
                    case HttpStatusCode.NotFound:
                        result.StatusCode = PutUsers_Contacts_Result.PutUsers_Contacts_StatusCode.NoThisUser;
                        break;
                    default:
                        result.StatusCode = PutUsers_Contacts_Result.PutUsers_Contacts_StatusCode.UnknownError;
                        break;
                }
#pragma warning disable CS0168 // Variable is declared but never used
            } catch (JsonException e) {
#pragma warning restore CS0168 // Variable is declared but never used
                result = new PutUsers_Contacts_Result() { StatusCode = PutUsers_Contacts_Result.PutUsers_Contacts_StatusCode.UnknownError };
            }
            return result;
        }

        public static async Task<PutUsers_InvitationResult> PutUsers_Invitation(string jwt, string un, string tk) {
            PutUsers_InvitationResult result;
            if (jwt.Equals("") || un.Equals("") || tk.Equals("")) {
                return new PutUsers_InvitationResult() { StatusCode = PutUsers_InvitationResult.PutUsers_InvitationStatusCode.UnknownError };
            }
            HttpClient client = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage() {
                Content = new HttpStringContent(JsonConvert.SerializeObject(new PutUsers_InvitationParams() {
                    invitation = tk
                })),
                Method = HttpMethod.Put,
                RequestUri = new Uri(API_HOST + URI_USERS_ + un + URI_INVITATION)
            };
            req.Headers["Authorization"] = jwt;
            HttpResponseMessage res = await client.SendRequestAsync(req);
            try {
                result = JsonConvert.DeserializeObject<PutUsers_InvitationResult>(res.Content.ToString());
                switch (res.StatusCode) {
                    case HttpStatusCode.Ok:
                        result.StatusCode = PutUsers_InvitationResult.PutUsers_InvitationStatusCode.Success;
                        break;
                    case HttpStatusCode.BadRequest:
                        result.StatusCode = PutUsers_InvitationResult.PutUsers_InvitationStatusCode.InvalidInvitation;
                        break;
                    case HttpStatusCode.Unauthorized:
                        result.StatusCode = PutUsers_InvitationResult.PutUsers_InvitationStatusCode.InvalidToken;
                        break;
                    case HttpStatusCode.NotFound:
                        result.StatusCode = PutUsers_InvitationResult.PutUsers_InvitationStatusCode.NoThisUser;
                        break;
                    case HttpStatusCode.Conflict:
                        result.StatusCode = PutUsers_InvitationResult.PutUsers_InvitationStatusCode.AlreadyContact;
                        break;
                    default:
                        result.StatusCode = PutUsers_InvitationResult.PutUsers_InvitationStatusCode.UnknownError;
                        break;
                }
#pragma warning disable CS0168 // Variable is declared but never used
            } catch (JsonException e) {
#pragma warning restore CS0168 // Variable is declared but never used
                result = new PutUsers_InvitationResult() { StatusCode = PutUsers_InvitationResult.PutUsers_InvitationStatusCode.UnknownError };
            }
            return result;
        }

        public static async Task<DeleteUsers_Contacts_Result> DeleteUsers_Contacts_(string jwt, string un, string cn) {
            DeleteUsers_Contacts_Result result;
            if (jwt.Equals("") || un.Equals("") || cn.Equals("")) {
                return new DeleteUsers_Contacts_Result() { StatusCode = DeleteUsers_Contacts_Result.DeleteUsers_Contacts_StatusCode.UnknownError };
            }
            HttpClient client = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage() {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(API_HOST + URI_USERS_ + un + URI_CONTACTS_ + cn)
            };
            req.Headers["Authorization"] = jwt;
            HttpResponseMessage res = await client.SendRequestAsync(req);
            try {
                result = JsonConvert.DeserializeObject<DeleteUsers_Contacts_Result>(res.Content.ToString());
                switch (res.StatusCode) {
                    case HttpStatusCode.Ok:
                        result.StatusCode = DeleteUsers_Contacts_Result.DeleteUsers_Contacts_StatusCode.Success;
                        break;
                    case HttpStatusCode.Unauthorized:
                        result.StatusCode = DeleteUsers_Contacts_Result.DeleteUsers_Contacts_StatusCode.InvalidToken;
                        break;
                    case HttpStatusCode.NotFound:
                        result.StatusCode = DeleteUsers_Contacts_Result.DeleteUsers_Contacts_StatusCode.NoThisUser;
                        break;
                    default:
                        result.StatusCode = DeleteUsers_Contacts_Result.DeleteUsers_Contacts_StatusCode.UnknownError;
                        break;
                }
#pragma warning disable CS0168 // Variable is declared but never used
            } catch (JsonException e) {
#pragma warning restore CS0168 // Variable is declared but never used
                result = new DeleteUsers_Contacts_Result() { StatusCode = DeleteUsers_Contacts_Result.DeleteUsers_Contacts_StatusCode.UnknownError };
            }
            return result;
        }

        public static async Task<IBuffer> GetAvatar(string url) {
            if (url.Equals("")) return null;
            HttpClient client = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage() {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };
            HttpResponseMessage res = await client.SendRequestAsync(req);
            switch (res.StatusCode) {
                case HttpStatusCode.Ok:
                    return await res.Content.ReadAsBufferAsync();
                default:
                    return null;
            }
        }
    }

    // Parameter models for request
    public class PostUsersParams {
        public string username;
        public string password;
    }
    public class PostAuthLoginParams {
        public string username;
        public string password;
    }
    public class PutUsers_Contacts_Params {
        public string remark;
        public int block_level;
    }
    public class PostUsers_InvitationParams {
        public string message;
    }
    public class PutUsers_InvitationParams {
        public string invitation;
    }
    public class PostUsers_MessageParams {
        public int type;
        public string content;
    }
    public class PostChatGroupParams {
        public string groupname;
    }
    public class GetChatGroup_Params {
        public string _id;
        public string groupname;
    }
    public class PostChatGroup_MembersParams {
        public List<string> members;
    }
    public class PutChatGroup_Params {
        public string groupname;
    }
    public class PutChatGroup_Members_Params {
        public string group_nick;
    }
    public class PostChatGroup_MessagesParams {
        public int type;
        public string content;
    }
    public class PutUser_Params {
        public string nickname;
        public string password;
        public string old_password;
        public string avatar;
        public int gender;
        public int region;
    }
    //public class GetUser_Param
    //{

    //}
    // Result for functions' returns
    public class PostUsersResult {
        public enum PostUsersStatusCode { Success, InvalidParams, ExistingUser, UnknownError };
        public PostUsersStatusCode StatusCode;
    }
    public class PostAuthLoginResult {
        public enum PostAuthLoginStatusCode { Success, Failure, UnknownError };
        public PostAuthLoginStatusCode StatusCode;
        public string jwt;
    }
    public class GetUsers_ContactsResult {
        public enum GetUsers0ContactsStatusCode { Success, InvalidToken, NoThisUser, UnknownError };
        public GetUsers0ContactsStatusCode StatusCode;
        public List<ContactItem> contacts;
        public class ContactItem {
            public Contact contact;
            public string remark;
            public int block_level;
            public class Contact {
                public string _id;
                public string username;
                public string nickname;
                public string avatar;
            }
        }
    }
    public class PutUsers_Contacts_Result {
        public enum PutUsers_Contacts_StatusCode { Success, InvalidParams, InvalidToken, NoThisUser, UnknownError };
        public PutUsers_Contacts_StatusCode StatusCode;
    }
    public class DeleteUsers_Contacts_Result {
        public enum DeleteUsers_Contacts_StatusCode { Success, InvalidToken, NoThisUser, UnknownError };
        public DeleteUsers_Contacts_StatusCode StatusCode;
    }
    public class PostUsers_InvitationResult {
        public enum PostUsers_InvitationStatusCode { Success, InvalidToken, NoThisUser, AlreadyContact, UnknownError };
        public PostUsers_InvitationStatusCode StatusCode;
    }
    public class PutUsers_InvitationResult {
        public enum PutUsers_InvitationStatusCode { Success, InvalidInvitation, InvalidToken, NoThisUser, AlreadyContact, UnknownError };
        public PutUsers_InvitationStatusCode StatusCode;
    }
    public class PostUsers_MessageResult {
        public enum PostUsers_MessageStatusCode { Success, InvalidParams, InvalidToken, NoThisUser, UnknownError };
        public PostUsers_MessageStatusCode StatusCode;
        public string _id;
        public string sender;
        public string receiver;
        public bool to_group;
        public int type;
        public long time;
        public string content;
    }
    public class PostChatGroupResult {
        public enum PostChatGroupStatusCode { Success, InvalidParams, InvalidToken, UnknownError };
        public PostChatGroupStatusCode StatusCode;
        public string _id;
        public string group_name;
        public List<Member> members;
        public class Member {
            public string member;
            public string group_nick;
        }
    }
    public class GetChatGroup_Result {
        public enum GetChatGroup_StatusCode { Success, InvalidToken, NoThisGroup, UnknownError };
        public GetChatGroup_StatusCode StatusCode;
    }
    public class GetChatGrouup_MembersResult {
        public enum GetChatGrouup_MembersStatusCode { Success, InvalidToken, NoThisGroup, UnknownError };
        public GetChatGrouup_MembersStatusCode StatusCode;

        public List<Member> members;
        public class Member {
            public member1 member;
            public class member1 {
                string _id;
                string username;
                string nickname;
                string avatar;
            }
            public string group_nick;
        }
    }
    public class PostChatGroup_MembersResult {
        public enum PostChatGroup_MembersStatusCode { Success, InvalidParams, InvalidToken, NoThisGroup, UnknownError };
        public PostChatGroup_MembersStatusCode StatusCode;
    }
    public class PutChatGroup_Result {
        public enum PutChatGroup_StatusCode { Success, InvalidParams, InvalidToken, NoThisGroup, UnknownError };
        public PutChatGroup_StatusCode StatusCode;
    }
    public class PutChatGroup_Members_Result {
        public enum PutChatGroup_Members_StatusCode { Success, InvalidParams, InvalidToken, NoThisGroup, UnknownError };
        public PutChatGroup_Members_StatusCode StatusCode;
    }
    public class PostChatGroup_MessagesResult {
        public enum PostChatGroup_MessagesStatusCode { Success, InvalidParams, InvalidToken, NoThisGroup, UnknownError };
        public PostChatGroup_MessagesStatusCode StatusCode;
        public string _id;
        public string sender;
        public string reeiver;
        public bool to_group;
        public int type;
        public string time;
        public string content;
    }
    public class GetUsers_Result {
        public enum GetUsers_StatusCode { Success, NoThisUser, UnknownError };
        public GetUsers_StatusCode StatusCode;
        public string _id;
        public string username;
        public string nickname;
        public string avatar;
        public int gender;
        public int region;
    }
    public class PutUsers_Result {
        public enum PutUser_StatusCode { Success, InvalidParams, InvalidToken, NoThisGroup, UnknownError };
        public PutUser_StatusCode StatusCode;
    }
    public class PostUsers_AvatarResult {
        public enum PostUsers_AvatarStatusCode { Success, InvalidContent, InvalidToken, NoThisUser, UnknownError };
        public PostUsers_AvatarStatusCode StatusCode;
    }
}
