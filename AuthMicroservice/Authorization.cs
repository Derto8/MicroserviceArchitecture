using AuthMicroservice.AuthClassies;
using AuthMicroservice.Interfaces;
using DBContext;
using DBContext.Interfaces;
using DBContext.Models;
using DBContext.RepositoryServices;
using Microsoft.AspNetCore.Mvc;

namespace AuthMicroservice
{
    public class Authorization : IAuthorization
    {
        private readonly ILogger<Authorization> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public Authorization(
            ILogger<Authorization> logger,
            ApplicationContext context,
            IConfiguration configuration,
            ILogger<UserRepository> loggerRepo)
        {
            _logger = logger;
            _configuration = configuration.GetSection("Authorization");

            _userRepository = new UserRepository(context, loggerRepo);

            //var scope = scopeFactory.CreateScope();
            //_userRepository = new UserRepository(
            //    scope.ServiceProvider.GetRequiredService<ApplicationContext>(),
            //    loggerRepo);
        }

        public async Task<IResult> AuthorizationMethod(string login, string password, CancellationToken cancellationToken)
        {
            Users user = await _userRepository.AuthorizationAsync(login, password, cancellationToken);

            if (user != null)
            {
                IResult jwt = GenerateJWT.Generate(user, _configuration, cancellationToken);

                _logger.LogInformation($"Юзер авторизировался: {login}");

                return jwt;
            }
            _logger.LogInformation($"Юзер не смог авторизироваться\n" +
                $"Login: {login}, Pass: {password}");

            return null;
        }
    }
}
