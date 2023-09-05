using DBContext;
using DBContext.Interfaces;
using DBContext.RepositoryServices;
using DTOs.AuthDTOs;
using IntraVisionTestTask.ConfigureOptions.Microservices;
using IntraVisionTestTask.MicroservicesRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IntraVisionTestTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthMicroserviceController : ControllerBase
    {
        private IUserRepository _userRepository { get; set; }
        private ILogger<AuthMicroserviceController> _logger { get; set; }
        private AuthMicroserviceOptions _authOptions { get; set; }
        public AuthMicroserviceController(ILogger<AuthMicroserviceController> logger,
            ILogger<UserRepository> loggerRepo,
            ApplicationContext context,
            IOptions<AuthMicroserviceOptions> authOptions)
        {
            _logger = logger;
            _userRepository = new UserRepository(context, loggerRepo);
            _authOptions = authOptions.Value;
        }

        [HttpPost, Route("auth/{login}/{password}")]
        public async Task<JWT> Authorize(string login, string password, CancellationToken cancellationToken)
        {
            JWT jwt = await AuthMicroserviceRequests.Authorize(login, password, _authOptions, cancellationToken);
            HttpContext.Session.SetString("token", jwt.access_token);
            return jwt;
        }
    }
}
