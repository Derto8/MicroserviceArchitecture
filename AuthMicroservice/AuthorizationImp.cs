using AuthMicroservice.Authorization;
using AuthMicroservice.Authorization.Utils;
using AuthMicroservice.Interfaces;
using DBContext;
using DBContext.Interfaces;
using DBContext.Models;
using DBContext.RepositoryServices;
using Helpers.JWTValidate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace AuthMicroservice
{
    /// <summary>
    /// класс имплементирующий интефейс IAuthorize
    /// </summary>
    public class AuthorizationImp : IAuthorize
    {
        private readonly ILogger<AuthorizationImp> _logger;
        private readonly IUserRepository _userRepository;
        private IOptions<AuthOptions> _authOptions;
        public AuthorizationImp(
            ILogger<AuthorizationImp> logger,
            ApplicationContext context,
            ILogger<UserRepository> loggerRepo,
            IOptions<AuthOptions> options)
        {
            _logger = logger;
            _userRepository = new UserRepository(context, loggerRepo);
            _authOptions = options;
        }

        public async Task<IResult> AuthorizationMethod(string login, string password, CancellationToken cancellationToken)
        {
            Users user = await _userRepository.AuthorizationAsync(login, password, cancellationToken);

            if (user != null)
            {
                IResult jwt = GenerateJWT.Generate(user, _authOptions);

                return jwt;
            }

            return null;
        }

        public async Task RegistrationMethod(string login, string password, CancellationToken cancellationToken)
        {
            await _userRepository.RegistrationAsync(login, password, cancellationToken);
        }
    }
}
