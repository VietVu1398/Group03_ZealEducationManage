using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ZealEducationManager.Controllers
{
    [Authorize]
    public class DashboardController : Controller
	{
		[Route("home")]
		public IActionResult Index()
		{
			return View();
		}
        [Route("message")]
        public IActionResult Message( )
        {
            return View();
        }
    }
}
