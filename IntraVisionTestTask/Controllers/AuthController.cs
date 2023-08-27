using AuthMicroservice.Controllers;
using DBContext.Models;
using IntraVisionTestTask.DTOs;
using IntraVisionTestTask.Extensions;
using IntraVisionTestTask.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IntraVisionTestTask.Controllers
{
    public class AuthController : Controller
    {
        private ILogger<AuthController> _logger;
        private AuthorizationMicroserviceOptions _microserviceOpt;

        public AuthController(
            ILogger<AuthController> logger,
            IOptions<AuthorizationMicroserviceOptions> microserviceOpt)
        {
            _logger = logger;
            _microserviceOpt = microserviceOpt.Value;
        }

        public IActionResult Authorize()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Authorize(string Login, string Password, CancellationToken token)
        {
            if (ModelState.IsValid)
            {
                JWT jwt = await PostRequests.Authorize(Login, Password, _microserviceOpt);
                if (jwt != null)
                {
                    HttpContext.Session.SetString("token", jwt.access_token);
                    //HttpContext.Session.Set<JWT>("", jwt);

                    return RedirectToAction("GetAll", "Drinks", token);
                }
                else
                {
                    ViewBag.Error = "Такого пользователя не существует!";
                    return View(token);
                }
            }
            return View(Login, Password);
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(string Login, string Password, CancellationToken token)
        {
            if (ModelState.IsValid)
            {
                bool status = await PostRequests.Registration(Login, Password, _microserviceOpt);
                if(status)
                    return RedirectToAction("Authorize", token);
                else
                {
                    ViewBag.Error = "Такой пользователь уже существует!";
                    return View(token);
                }
            }
            return View(token);
        }
    }
}
