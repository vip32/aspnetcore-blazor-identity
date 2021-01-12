using WebApp.Shared;
using System.Threading.Tasks;

namespace WebApp.Client
{
    public interface IAuthenticationClient
    {
        Task Login(UserLoginModel model);

        Task Register(UserRegisterModel model);

        Task Logout();

        Task<UserInfoModel> GetUserInfo();
    }
}
