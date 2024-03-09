using Microsoft.AspNetCore.Mvc;

namespace Hotel.ATR.Portal.Controllers
{
    public class RoomListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
