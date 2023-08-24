﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace AuthMicroservice.Authorization.Utils.Services
{
    public class JwtBearerOptionsConfiguration : IConfigureNamedOptions<JwtBearerOptions>
    {
        public JwtBearerOptionsConfiguration(
            IOptions<AuthOptions> authOptions,
            ILogger<JwtBearerOptions> logger)
        {
            _authOptions = authOptions.Value;
            _logger = logger;
        }

        private AuthOptions _authOptions { get; }
        private ILogger<JwtBearerOptions> _logger { get; }

        public void Configure(string name, JwtBearerOptions options)
        {
            Configure(options);
        }

        public void Configure(JwtBearerOptions options)
        {
            SecurityKeyProvider securityKeyProvider = new SecurityKeyProvider(_authOptions.KEY);

            options.IncludeErrorDetails = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _authOptions.ISSUER,
                ValidateAudience = true,
                ValidAudience = _authOptions.AUDIENCE,
                ValidateLifetime = true,
                IssuerSigningKey = securityKeyProvider.GetSymmetricSecurityKey(_authOptions.KEY),
                ValidateIssuerSigningKey = true,
            };
            options.Events = new JwtBearerEvents
            {
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
