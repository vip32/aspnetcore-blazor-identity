using WebApp.Server.Models;
using WebApp.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Security.Claims;

namespace WebApp.Server.Controllers
{
    [Route("api/auth/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AuthenticationController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            if (user == null) return BadRequest("User does not exist");

            var singInResult = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!singInResult.Succeeded) return BadRequest("Invalid password");

            await signInManager.SignInAsync(user, model.RememberMe);

            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await userManager.CreateAsync(user, model.Password);
            await userManager.AddClaimAsync(user, new(ClaimTypes.GivenName, $"{model.FirstName} {model.LastName}"));
            await userManager.AddClaimAsync(user, new("registeredDate", DateTime.UtcNow.ToString("o")));

            if (!result.Succeeded) return BadRequest(result.Errors.FirstOrDefault()?.Description);

            return await Login(new()
            {
                UserName = model.UserName,
                Password = model.Password
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }

        [HttpGet]
        [Authorize]
        public UserInfoModel UserInfo()
        {
            //var user = await userManager.GetUserAsync(HttpContext.User);
            return new()
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                UserName = User.Identity.Name,
                Claims = User.Claims
                    .ToDictionary(c => c.Type, c => c.Value)
            };
        }
    }
}
