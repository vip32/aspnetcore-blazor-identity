using WebApp.Shared;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WebApp.Client
{
    public class AuthenticationClient : IAuthenticationClient
    {
        private readonly HttpClient httpClient;

        public AuthenticationClient(HttpClient client)
        {
            httpClient = client;
        }

        public async Task Login(UserLoginModel model)
        {
            //var stringContent = new StringContent(JsonSerializer.Serialize(loginParameters), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsJsonAsync("api/auth/login", model);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await response.Content.ReadAsStringAsync());

            response.EnsureSuccessStatusCode();
        }

        public async Task Logout()
        {
            var response = await httpClient.PostAsync("api/auth/logout", null);

            response.EnsureSuccessStatusCode();
        }

        public async Task Register(UserRegisterModel model)
        {
            //var stringContent = new StringContent(JsonSerializer.Serialize(registerParameters), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsJsonAsync("api/auth/register", model);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await response.Content.ReadAsStringAsync());

            response.EnsureSuccessStatusCode();
        }

        public async Task<UserInfoModel> GetUserInfo()
        {
            return await httpClient.GetFromJsonAsync<UserInfoModel>("api/auth/userinfo");
        }
    }
}
