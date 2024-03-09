using Hotel.ATR.Portal.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;

namespace Hotel.ATR.Portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }

        [Authorize]
        public IActionResult Contact() { return View(); }
        public IActionResult Login(string ReturnUrl) { 
            ViewBag.ReturnUrl = ReturnUrl;
            return View(); }
        [HttpPost]
        public IActionResult Login(string login, string password, string ReturnUrl)
        {
            if (login == "admin" && password == "admin")
            {
                var claims = new List<Claim>
                { new Claim(ClaimTypes.Name ,login) };
                var claimsIdentity = new ClaimsIdentity(claims, "Login");

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                if (string.IsNullOrEmpty(ReturnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Redirect(ReturnUrl);
                }
            }
            return View();
        }

        public IActionResult Logout() {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index"); }
        public IActionResult Index(string culture)

        {
            if (!string.IsNullOrWhiteSpace(culture))
            {
                CultureInfo.CurrentCulture=new CultureInfo(culture);
                CultureInfo.CurrentUICulture=new CultureInfo(culture);
            }
            HttpContext.Session.SetString("iin", "880111300392");

            var sessionData = HttpContext.Session.GetString("iin");



            CookieOptions options = new CookieOptions();  
            options.Expires = DateTime.Now.AddSeconds(100);
            Response.Cookies.Append("iin", "880111300392", options);
            var value = Request.Cookies["iin"];
            User user = new User();
            user.email = "ok@ok.kz";
            _logger.LogError("У пользователья {email} возникла ошибка {errorMessage}", user.email, "Ошибка пользователя");
            Stopwatch sw = new Stopwatch();
            _logger.LogInformation("Обращаемся к сервису");
            sw.Start();
            Thread.Sleep(1000);
            //TODO вызов чужого сервера 
            sw.Stop();
            _logger.LogInformation("Сервис отработал за {ElapsedMilliseconds}", sw.ElapsedMilliseconds);
            _logger.LogInformation("Logging Information");
            _logger.LogError("Log Error");
            _logger.LogWarning("Log Warning");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}