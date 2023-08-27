using AuthMicroservice.Controllers;
using IntraVisionTestTask.DTOs;
using IntraVisionTestTask.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IntraVisionTestTask.Controllers
{
    public class AuthController : Controller
    {
        private ILogger<AuthController> _logger;
        private IConfiguration _configuration;

        public AuthController(
            ILogger<AuthController> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Authorize()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Authorize(string Login, string Password)
        {
            if (ModelState.IsValid)
            {
                JWT jwt = await PostRequests.Authorize(Login, Password, _configuration);
                if (jwt != null)
                {
                    HttpContext.Session.SetString("token", jwt.access_token);
                    return RedirectToAction("GetAll", "Drinks");
                }
                else
                {
                    ViewBag.Error = "Такого пользователя не существует!";
                    return View();
                }
            }
            return View(Login, Password);
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(string Login, string Password)
        {
            if (ModelState.IsValid)
            {
                bool status = await PostRequests.Registration(Login, Password, _configuration);
                if(status)
                    return RedirectToAction("Authorize");
                else
                {
                    ViewBag.Error = "Такой пользователь уже существует!";
                    return View();
                }
            }
            return View(Login, Password);
        }
    }
}
