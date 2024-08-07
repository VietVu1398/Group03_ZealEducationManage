using Microsoft.AspNetCore.Mvc;

namespace ZealEducationManager.Controllers
{
	public class SecurityController : Controller
	{
			public IActionResult Login()
		{
			return View();
		}
	}
}
