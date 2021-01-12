using System.Collections.Generic;

namespace WebApp.Shared
{
    public class UserInfoModel
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public Dictionary<string, string> Claims { get; set; }
    }
}
