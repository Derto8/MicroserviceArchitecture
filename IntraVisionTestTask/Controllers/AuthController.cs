using AuthMicroservice.Controllers;
using DBContext;
using DBContext.Interfaces;
using DBContext.Models;
using DBContext.RepositoryServices;
using IntraVisionTestTask.ConfOptions;
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
        private IConfiguration _configuration;

        public AuthController(
            ILogger<AuthController> logger,
            IOptions<AuthorizationMicroserviceOptions> microserviceOpt,
            ApplicationContext context,
            ILogger<UserRepository> loggerRepo,
            IConfiguration conf)
        {
            _logger = logger;
            _microserviceOpt = microserviceOpt.Value;
            _userRepository = new UserRepository(context, loggerRepo);
            _configuration = conf;
        }

        public IActionResult Authorize()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> AuthUser([FromBody]AuthUser authUser, CancellationToken cancellationToken)
        {
            JWT jwt = await PostRequests.Authorize(authUser.Login, authUser.Password, _microserviceOpt);
            Users user = await _userRepository.GetAsync(jwt.userId, cancellationToken);
            if (jwt != null && user != null)
            {
                HttpContext.Session.Set("token", jwt.access_token);
                HttpContext.Session.Set("user", user);

                var json = new JWTAndAddres
                {
                    jwt = jwt.access_token,
                    addres = _configuration["MainServerAddress"]
                };
                return Json(json);
            }
            else return Json($"{_configuration["MainServerAddress"]}/Auth/Authorize");
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

        /// <summary>
        /// Выход из аккаунта
        /// </summary>
        [HttpGet]
        public void LogOut()
        {
            HttpContext.Session.Remove("token");
            HttpContext.Session.Remove("user");
        }
    }
}
