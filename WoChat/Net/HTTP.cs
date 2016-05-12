using System;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace WoChat.Net {
    class HTTP {
        // Host
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
            switch (res.StatusCode) {
                case HttpStatusCode.Created:
                    return PostUsersResult.Success;
                case HttpStatusCode.BadRequest:
                    return PostUsersResult.InvalidParams;
                case HttpStatusCode.Conflict:
                    return PostUsersResult.ExistingUser;
                default:
                    return PostUsersResult.InvalidParams;
            }
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
            switch (res.StatusCode) {
                case HttpStatusCode.Ok:
                    return PostAuthLoginResult.Success;
                case HttpStatusCode.Unauthorized:
                    return PostAuthLoginResult.Failure;
                default:
                    return PostAuthLoginResult.Failure;
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

    // Result for functions' returns
    public enum PostUsersResult { Success, InvalidParams, ExistingUser };
    public enum PostAuthLoginResult { Success, Failure };
}
