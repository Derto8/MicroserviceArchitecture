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
            IServiceScopeFactory scopeFactory,
            IConfiguration configuration,
            ILogger<UserRepository> loggerRepo)
        {
            _logger = logger;
            _configuration = configuration.GetSection("Authorization");

            var scope = scopeFactory.CreateScope();
            _userRepository = new UserRepository(
                scope.ServiceProvider.GetRequiredService<ApplicationContext>(), 
                loggerRepo);

        }

        public async Task<IResult> AuthorizationMethod(string login, string password)
        {
            Users user = await _userRepository.AuthorizationAsync(login, password);

            if (user != null)
            {
                IResult jwt = GenerateJWT.Generate(user, _configuration);

                _logger.LogInformation($"Юзер авторизировался: {login}");

                return jwt;
            }
            _logger.LogInformation($"Юзер не смог авторизироваться\n" +
                $"Login: {login}, Pass: {password}");

            return null;
        }
    }
}
