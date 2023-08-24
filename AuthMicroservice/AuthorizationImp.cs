using AuthMicroservice.Authorization;
using AuthMicroservice.Authorization.Utils;
using AuthMicroservice.Interfaces;
using DBContext;
using DBContext.Interfaces;
using DBContext.Models;
using DBContext.RepositoryServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AuthMicroservice
{
    public class AuthorizationImp : IAuthorization
    {
        private readonly ILogger<AuthorizationImp> _logger;
        private readonly IUserRepository _userRepository;
        private AuthOptions _authOptions;
        public AuthorizationImp(
            ILogger<AuthorizationImp> logger,
            ApplicationContext context,
            ILogger<UserRepository> loggerRepo,
            IOptions<AuthOptions> options)
        {
            _logger = logger;
            _userRepository = new UserRepository(context, loggerRepo);
            _authOptions = options.Value;
        }

        public async Task<IResult> AuthorizationMethod(string login, string password, CancellationToken cancellationToken)
        {
            Users user = await _userRepository.AuthorizationAsync(login, password, cancellationToken);

            if (user != null)
            {
                IResult jwt = GenerateJWT.Generate(user, _authOptions, cancellationToken);

                _logger.LogInformation($"Юзер авторизировался: {login}");

                return jwt;
            }
            _logger.LogInformation($"Юзер не смог авторизироваться\n" +
                $"Login: {login}, Pass: {password}");

            return null;
        }
    }
}
