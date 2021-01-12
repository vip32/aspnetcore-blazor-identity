using WebApp.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApp.Client
{
    public class IdentityAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IAuthenticationClient authenticationClient;
        private UserInfoModel userInfoCache;

        public IdentityAuthenticationStateProvider(IAuthenticationClient client)
        {
            authenticationClient = client;
        }

        public async Task Login(UserLoginModel model)
        {
            await authenticationClient.Login(model);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task Register(UserRegisterModel model)
        {
            await authenticationClient.Register(model);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task Logout()
        {
            await authenticationClient.Logout();
            userInfoCache = null;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        private async Task<UserInfoModel> GetUser()
        {
            if (userInfoCache != null && userInfoCache.IsAuthenticated) return userInfoCache;
            userInfoCache = await authenticationClient.GetUserInfo();

            return userInfoCache;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            try
            {
                var userInfo = await GetUser();
                if (userInfo.IsAuthenticated)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, userInfo.UserName) }.Concat(userInfo.Claims.Select(c => new Claim(c.Key, c.Value)));
                    identity = new ClaimsIdentity(claims, "Server authentication");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Request failed:" + ex.ToString()); // TODO: user logger here
            }

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
    }
}
