using WebApp.Shared;
using System.Threading.Tasks;

namespace WebApp.Client
{
    public interface IAuthorizeApi
    {
        Task Login(UserLoginModel loginParameters);
        Task Register(UserRegisterModel registerParameters);
        Task Logout();
        Task<UserInfo> GetUserInfo();
    }
}
