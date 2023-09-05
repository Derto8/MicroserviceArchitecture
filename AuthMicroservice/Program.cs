using AuthMicroservice.Authorization;
using AuthMicroservice.Controllers;
using AuthMicroservice.Interfaces;
using DBContext;
using DBContext.Interfaces;
using GlobalExceptionHandling.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Shed.CoreKit.WebApi;
using System.Security.Cryptography;
using Microsoft.OpenApi.Models;
using Helpers.JWTValidate.KeyProviders;
using Helpers.JWTValidate.Interfaces;
using Helpers.JWTValidate;
using Helpers;

namespace AuthMicroservice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection(AuthOptions.Autorization));

            builder.Services.AddCorrelationToken();
            builder.Services.AddCors();

            builder.Services.AddMvcCore();

            builder.Services.AddSwaggerDocumentation();

            builder.Services.AddControllers();

            //подключение бд
            string conn = builder.Configuration.GetConnectionString("ConnectionDataBase");
            builder.Services.AddDbContext<ApplicationContext>(opt =>
            {
                opt.UseSqlServer(conn);
            });

            builder.Services.AddSingleton<IPublicKeyProvider, PublicKeyProvider>();
            builder.Services.AddTransient<IAuthorize, AuthorizationImp>();

            var app = builder.Build();

            app.MapControllers();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerDocumentation();
            }

            app.UseCorrelationToken();

            //корсы
            app.UseCors(builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            // миддлваль обработки ошибок
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.Run();
        }
    }
}