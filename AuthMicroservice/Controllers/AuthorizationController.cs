using AuthMicroservice.Interfaces;
using DBContext;
using DBContext.Interfaces;
using DBContext.Models;
using DBContext.RepositoryServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading;

namespace AuthMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            ILogger<AuthorizationImp> loggerAuth)
        {
            _logger = logger;
            _configuration = configuration.GetSection("Authorization");

            _authorization = new AuthorizationImp(
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

        [HttpGet(template: "Addsdgfd")]
        [Authorize]
        public string Gooo()
        {
            return "fdsf";
        }
    }
}
