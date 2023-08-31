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

        /// <summary>
        /// Показывает страницу с формой авторизации юзера
        /// </summary>
        /// <returns>Страница авторизации юзера</returns>
        public IActionResult Authorize()
        {
            return View();
        }

        /// <summary>
        /// Посылает POST-запрос к микросервису AuthMicroserice на авторизацию юзера,
        /// в теле запроса находится модель данных необходимая для авторизации.
        /// </summary>
        /// <param name="authUser">Модель данных юзера необходимых для его авторизации на сервере</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        /// <returns>При успешной авторизации, возвращает JWT-токен юзера
        /// а так же путь к редиректу на главную страницу</returns>
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

        /// <summary>
        /// Показывает страницу с формой регистрации юзера
        /// </summary>
        /// <returns>Страница регистрации юзера</returns>
        public IActionResult Registration()
        {
            return View();
        }

        /// <summary>
        /// Посылает POST-запрос к микросервису AuthMicroserice на регистрацию юзера,
        /// в теле запроса находится логин и пароль юзера, которые добавляются в бд.
        /// </summary>
        /// <param name="Login">Логин юзера</param>
        /// <param name="Password">Пароль</param>
        /// <param name="token">Токен отмены</param>
        /// <returns>Редирект на страницу авторизации</returns>
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
        /// Выход юзера с аккаунт (обращение к методу с клиента)
        /// </summary>
        [HttpGet]
        public void LogOut()
        {
            HttpContext.Session.Remove("token");
            HttpContext.Session.Remove("user");
        }
    }
}
