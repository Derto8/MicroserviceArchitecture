﻿using AuthMicroservice.Authorization;
using AuthMicroservice.Interfaces;
using DBContext;
using DBContext.Interfaces;
using DBContext.Models;
using DBContext.RepositoryServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace AuthMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly ILogger<AuthorizationController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAuthorization _authorization;
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

        //  [HttpPost(template: "AuthMethod")]
        [HttpPost, Route("authuser/{login}/{password}")]
        public async Task<IResult> AuthorizationMethod(string login, string password, CancellationToken cancellationToken)
        {
            return await _authorization.AuthorizationMethod(login, password, cancellationToken);
        }

        [HttpPost, Route("registration/{login}/{password}")]
        public async Task<HttpStatusCode> RegistrationMethod(string login, string password, CancellationToken cancellationToken)
        {
            return await _authorization.RegistrationMethod(login, password, cancellationToken);
        }

        [HttpGet(template: "Addsdgfd")]
        [Authorize]
        public string Gooo()
        {
            return "fdsf";
        }
    }
}
