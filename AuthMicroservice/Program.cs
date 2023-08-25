using AuthMicroservice.Authorization.Utils.Services;
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
using AuthMicroservice.Authorization.Utils.KeyProviders;

namespace AuthMicroservice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddCorrelationToken();
            builder.Services.AddCors();

            builder.Services.AddMvcCore();

            builder.Services.AddControllers();

            builder.Services.AddSingleton<IPublicKeyProvider, PublicKeyProvider>();

            //подключение бд
            string conn = builder.Configuration.GetConnectionString("ConnectionDataBase");
            builder.Services.AddDbContext<ApplicationContext>(opt =>
            {
                opt.UseSqlServer(conn);
            });

            builder.Services.AddTransient<IAuthorization, AuthorizationImp>();
            builder.Services.AddTransient<HttpClient>();

            var app = builder.Build();

            app.MapControllers();

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

            //привязываю реализацию к конечной точке
            app.UseWebApiEndpoint<IAuthorization>();

            app.Run();
        }
    }
}