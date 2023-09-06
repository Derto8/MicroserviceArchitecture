using AuthMicroservice.Authorization;
using AuthMicroservice.Interfaces;
using DBContext;
using DBContext.Interfaces;
using DBContext.Models;
using DBContext.RepositoryServices;
using Helpers.JWTValidate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Threading;

namespace AuthMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly ILogger<AuthorizationController> _logger;
        private readonly IAuthorize _authorization;
        public AuthorizationController(
            ILogger<AuthorizationController> logger,
            ApplicationContext context,
            IOptions<AuthOptions> authOptions,
            ILogger<UserRepository> loggerRepo,
            ILogger<AuthorizationImp> loggerAuth)
        {
            _logger = logger;

            _authorization = new AuthorizationImp(
                loggerAuth,
                context,
                loggerRepo,
                authOptions);
        }


        /// <summary>
        /// Метод авторизации юзера
        /// </summary>
        /// <param name="login">логин</param>
        /// <param name="password">пароль</param>
        /// <param name="cancellationToken">токен отмены задачи</param>
        /// <returns>JSON-файл с jwt и id записи в бд пользователя</returns>
        [HttpPost, Route("AuthUser/{login}/{password}")]
        public async Task<IResult> AuthorizationMethod(string login, string password, CancellationToken cancellationToken)
        {
            return await _authorization.AuthorizationMethod(login, password, cancellationToken);
        }

        /// <summary>
        /// Метод авторизации юзера
        /// </summary>
        /// <param name="login">логин</param>
        /// <param name="password">пароль</param>
        /// <param name="cancellationToken">токен отмены задачи</param>
        [HttpPost, Route("RegUser/{login}/{password}")]
        public async Task RegistrationUser(string login, string password, CancellationToken cancellationToken)
        {
            await _authorization.RegistrationMethod(login, password, cancellationToken);
        }
    }
}
