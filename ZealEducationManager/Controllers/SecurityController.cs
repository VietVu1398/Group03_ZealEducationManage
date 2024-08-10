using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using ZealEducationManager.Entities;
using ZealEducationManager.Models.SecurityViewModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
namespace ZealEducationManager.Controllers
{
    public class SecurityController : Controller
	{
        private readonly ZealEducationManagerContext _context;
        private readonly ILogger<SecurityController> _logger;
        public SecurityController(ZealEducationManagerContext context, ILogger<SecurityController> logger)
        {
            _context = context;
            _logger = logger;
        }
        public IActionResult Login()
		{
            if(User.Identity.IsAuthenticated)
             {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
		}
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == model.Username);

            if (user != null && model.Password == user.Password)
            {
                var claims = new[]
                {
                new Claim(ClaimTypes.Name, user.Username)
            };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Dashboard");
            }

            ModelState.AddModelError("", "Username or Password are wrong");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
