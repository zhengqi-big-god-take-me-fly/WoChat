using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace WoChat.Net {
    class HTTP {
        // Host
        //TODO: Should be change to Internet IP address
        public static string API_HOST = "http://localhost:3000";

        // Uri
        public static string URI_USERS = "/users";
        public static string URI_AUTH_LOGIN = "/auth/login";

        public static async Task<PostUsersResult> PostUsers(string un, string pw) {
            HttpClient client = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage() {
                Content = new HttpStringContent(JsonConvert.SerializeObject(new PostUsersParams() {
                    username = un,
                    //TODO: MD5 encryption
                    password = pw
                }), UnicodeEncoding.Utf8, "application/json"),
                Method = HttpMethod.Post,
                RequestUri = new Uri(API_HOST + URI_USERS)
            };
            HttpResponseMessage res = await client.SendRequestAsync(req);
            PostUsersResult result = new PostUsersResult();
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
            HttpClient client = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage() {
                Content = new HttpStringContent(JsonConvert.SerializeObject(new PostAuthLoginParams() {
                    username = un,
                    //TODO: MD5 encryption
                    password = pw
                }), UnicodeEncoding.Utf8, "application/json"),
                Method = HttpMethod.Post,
                RequestUri = new Uri(API_HOST + URI_AUTH_LOGIN)
            };
            HttpResponseMessage res = await client.SendRequestAsync(req);
            PostAuthLoginResult result;
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
            } catch (JsonException e) {
                result = new PostAuthLoginResult() { StatusCode = PostAuthLoginResult.PostAuthLoginStatusCode.UnknownError };
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
}
