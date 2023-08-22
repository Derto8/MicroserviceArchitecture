using AuthMicroservice.AuthClassies;
using AuthMicroservice.Interfaces;
using DBContext;
using DBContext.Interfaces;
using DBContext.Models;
using DBContext.RepositoryServices;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading;

namespace AuthMicroservice.Controllers
{
    [ApiController]
    [Route("apiMicro/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly ILogger<AuthorizationController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IAuthorization _authorization;
        public AuthorizationController(
            ILogger<AuthorizationController> logger,
            ApplicationContext context,
            IConfiguration configuration,
            ILogger<UserRepository> loggerRepo,
            ILogger<Authorization> loggerAuth)
        {
            _logger = logger;
            _configuration = configuration.GetSection("Authorization");

            _authorization = new Authorization(
                loggerAuth,
                context,
                configuration,
                loggerRepo);
        }

        //  [HttpPost(template: "AuthMethod")]
        [HttpPost, Route("authuser/{login}/{password}")]
        public async Task<IResult> AuthorizationMethod(string login, string password, CancellationToken cancellationToken)
        {
            return await _authorization.AuthorizationMethod(login, password, cancellationToken);
        }
    }
}
