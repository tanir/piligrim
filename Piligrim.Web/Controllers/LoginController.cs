using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Piligrim.Web.Configuration;
using Microsoft.AspNetCore.Authentication;

namespace Piligrim.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IOptions<AppSettings> options;

        public LoginController(IOptions<AppSettings> options)
        {
            this.options = options;
        }
        public async Task<IActionResult> Index(string key)
        {
            if (this.options.Value.Key != key)
            {
                return this.NotFound();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Moderator"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var props = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMonths(1),
                AllowRefresh = true
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(identity), props);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}