using Microsoft.AspNetCore.Identity;
using System;

namespace WebApp.Server.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        //[PersonalData]
        //public string FirstName { get; set; }

        //[PersonalData]
        //public string LastName { get; set; }
    }
}
