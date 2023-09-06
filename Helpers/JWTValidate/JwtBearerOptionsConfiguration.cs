using Helpers.JWTValidate.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.JWTValidate
{
    /// <summary>
    /// Класс настройки валидации jwt
    /// </summary>
    public class JwtBearerOptionsConfiguration : IConfigureNamedOptions<JwtBearerOptions>
    {
        public JwtBearerOptionsConfiguration(
            IOptions<AuthOptions> authOptions,
            ILogger<JwtBearerOptions> logger,
            IServiceProvider serviceProvider)
        {
            _authOptions = authOptions.Value;
            _logger = logger;
            PublicKeyProvider = serviceProvider.GetRequiredService<IPublicKeyProvider>();
        }

        private AuthOptions _authOptions { get; }
        private ILogger<JwtBearerOptions> _logger { get; }
        private IPublicKeyProvider PublicKeyProvider { get; }

        /// <summary>
        /// Конфигурация настройки валидации jwt
        /// </summary>
        /// <param name="name">??</param>
        /// <param name="options">jwt опции</param>
        public void Configure(string? name, JwtBearerOptions options)
        {
            Configure(options);
        }

        /// <summary>
        /// Конфигурация настройки валидации jwt
        /// </summary>
        /// <param name="options">jwt опции</param>
        public void Configure(JwtBearerOptions options)
        {
            options.IncludeErrorDetails = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _authOptions.ISSUER,
                ValidateAudience = true,
                ValidAudience = _authOptions.AUDIENCE,
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                ValidateLifetime = true,
                IssuerSigningKey = PublicKeyProvider.GetKey(),
                ValidateIssuerSigningKey = true,
            };
            //лог событий
            options.Events = new JwtBearerEvents
            {
                //при ошибке валидации токена
                OnAuthenticationFailed = context =>
                {
                    context.NoResult();
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.ContentType = "application/json";
                    var exception = context.Exception;
                    _logger.LogError(new EventId(exception.HResult),
                        exception, exception.Message);

                    return Task.CompletedTask;
                }
            };
        }
    }
}
