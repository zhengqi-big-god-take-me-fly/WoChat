using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;
using WoChat.Utils;

namespace WoChat.Net {
    class HTTP {
        // Host
        //TODO: Should be change to Internet IP address
        public static string API_HOST = "http://tidyzq.com:3000";

        // Uri
        public static string URI_USERS = "/users";
        public static string URI_USERS_ = "/users/";
        public static string URI_AUTH_LOGIN = "/auth/login";
        public static string URI_CONTACTS = "/contacts";
        public static string URI_CONTACTS_ = "/contacts/";
        public static string URI_INVITATION = "/invitation";

        public static async Task<PostUsersResult> PostUsers(string un, string pw) {
            PostUsersResult result;
            if (un.Equals("") || pw.Equals("")) {
                return new PostUsersResult() { StatusCode = PostUsersResult.PostUsersStatusCode.InvalidParams };
            }
            HttpClient client = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage() {
                Content = new HttpStringContent(JsonConvert.SerializeObject(new PostUsersParams() {
                    username = un,
                    password = MD5.Hash(pw)
                }), UnicodeEncoding.Utf8, "application/json"),
                Method = HttpMethod.Post,
                RequestUri = new Uri(API_HOST + URI_USERS)
            };
            HttpResponseMessage res = await client.SendRequestAsync(req);
            result = new PostUsersResult();
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

        public static async Task<PostAuthLoginResult> PostAuthLogin(string un, string pw) {
            PostAuthLoginResult result;
            if (un.Equals("") || pw.Equals("")) {
                return new PostAuthLoginResult() { StatusCode = PostAuthLoginResult.PostAuthLoginStatusCode.UnknownError };
            }
            HttpClient client = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage() {
                Content = new HttpStringContent(JsonConvert.SerializeObject(new PostAuthLoginParams() {
                    username = un,
                    password = MD5.Hash(pw)
                }), UnicodeEncoding.Utf8, "application/json"),
                Method = HttpMethod.Post,
                RequestUri = new Uri(API_HOST + URI_AUTH_LOGIN)
            };
            HttpResponseMessage res = await client.SendRequestAsync(req);
            try {
                result = JsonConvert.DeserializeObject<PostAuthLoginResult>(res.Content.ToString());
                switch (res.StatusCode) {
                    case HttpStatusCode.Ok:
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

        public static async Task<GetUsers_ContactsResult> Getusers_Contacts(string jwt, string un) {
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

        public static async Task<PostUsers_InvitationResult> PostUsers_Invitation(string jwt, string cn, string m) {
            PostUsers_InvitationResult result;
            if (jwt.Equals("") || cn.Equals("")) {
                return new PostUsers_InvitationResult() { StatusCode = PostUsers_InvitationResult.PostUsers_InvitationStatusCode.UnknownError };
            }
            HttpClient client = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage() {
                Content = new HttpStringContent(JsonConvert.SerializeObject(new PostUsers_InvitationParams() {
                    message = m
                }), UnicodeEncoding.Utf8, "application/json"),
                Method = HttpMethod.Post,
                RequestUri = new Uri(API_HOST + URI_USERS_ + cn + URI_INVITATION)
            };
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
}
