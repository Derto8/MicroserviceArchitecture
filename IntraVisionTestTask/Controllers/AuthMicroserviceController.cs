using DBContext;
using DBContext.RepositoryServices;
using DTOs.AuthDTOs;
using IntraVisionTestTask.ConfigureOptions.Microservices;
using IntraVisionTestTask.MicroservicesRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace IntraVisionTestTask.Controllers
{
    /// <summary>
    /// Контроллер для запросов к микросервису AuthMicroservice
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthMicroserviceController : ControllerBase
    {
        private ILogger<AuthMicroserviceController> _logger { get; set; }
        private AuthMicroserviceOptions _authOptions { get; set; }
        public AuthMicroserviceController(ILogger<AuthMicroserviceController> logger,
            IOptions<AuthMicroserviceOptions> authOptions)
        {
            _logger = logger;
            _authOptions = authOptions.Value;
        }

        [HttpPost, Route("auth/{login}/{password}")]
        public async Task<JWT> Authorize(string login, string password, CancellationToken cancellationToken)
        {
            JWT jwt = await AuthMicroserviceRequests.Authorize(login, password, _authOptions, cancellationToken);
            HttpContext.Session.SetString("token", jwt.access_token);
            return jwt;
        }

        [HttpPost, Route("reg/{login}/{password}")]
        public async Task<HttpStatusCode> Registration(string login, string password, CancellationToken cancellationToken)
        {
            return await AuthMicroserviceRequests.Registration(login, password, _authOptions, cancellationToken);
        }
    }
}
