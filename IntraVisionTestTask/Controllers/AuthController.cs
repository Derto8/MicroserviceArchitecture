using AuthMicroservice.Controllers;
using DBContext;
using DBContext.Interfaces;
using DBContext.Models;
using DBContext.RepositoryServices;
using IntraVisionTestTask.DTOs;
using IntraVisionTestTask.Extensions;
using IntraVisionTestTask.Requests;
using Microsoft.AspNetCore.Identity;
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
        private IUserRepository _userRepository;

        public AuthController(
            ILogger<AuthController> logger,
            IOptions<AuthorizationMicroserviceOptions> microserviceOpt,
            ApplicationContext context,
            ILogger<UserRepository> loggerRepo)
        {
            _logger = logger;
            _microserviceOpt = microserviceOpt.Value;
            _userRepository = new UserRepository(context, loggerRepo);
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
                Users user = await _userRepository.GetAsync(jwt.userId, token);
                if (jwt != null && user != null)
                {
                    HttpContext.Session.SetString("token", jwt.access_token);
                    HttpContext.Session.Set<Users>("user", user);

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
