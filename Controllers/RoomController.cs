using Hotel.ATR.Portal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.ATR.Portal.Controllers
{
    public class RoomController : Controller
    {
        private IWebHostEnvironment webHost;
        private readonly ILogger<RoomController> _logger;    
        public RoomController(IWebHostEnvironment webHost, ILogger<RoomController> _logger)
        {
            this.webHost = webHost;
            this._logger = _logger;

        }
        public IActionResult Index(int page,int counter)
        {
            _logger.LogInformation("Logging Information");
            _logger.LogError("Log Error");
            _logger.LogWarning("Log Warning");
            var user = new User() { email = "ok@ok.kz", name = "yevgeniy" };
            ViewBag.User = user;
            ViewData["user"]=user;
            TempData["user"]= user;
            return View(user);
        }
        public IActionResult RenderPartial() { return View(); }
        public IActionResult RoomDetails() { return View(); }
        public IActionResult RoomList() { return View(); }
        [HttpPost]
        public IActionResult SubscribeNewsletter(IFormFile userFile)
        {
            var data = Request.Form["email"];
            string path=Path.Combine(webHost.WebRootPath, userFile.FileName);
            using(var stream =new  FileStream(path, FileMode.Create))
            {
                userFile.CopyTo(stream);
            }
            //return View();
            return RedirectToAction("Index");
            //return View("~/Views/Home/Index.cshtml");
        }
    }
   
}
