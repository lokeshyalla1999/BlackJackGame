using Microsoft.AspNetCore.Mvc;

namespace BlackJackMVC.Controllers
{
    public class StartGame : Controller
    {
        /// <summary>
        /// Just Game start Page Controller
        /// </summary>
        /// <returns></returns>
        [HttpGet("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
