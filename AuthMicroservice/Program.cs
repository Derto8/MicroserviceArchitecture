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
using Microsoft.OpenApi.Models;

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
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            builder.Services.AddControllers();

            //подключение бд
            string conn = builder.Configuration.GetConnectionString("ConnectionDataBase");
            builder.Services.AddDbContext<ApplicationContext>(opt =>
            {
                opt.UseSqlServer(conn);
            });

            builder.Services.AddSingleton<IPublicKeyProvider, PublicKeyProvider>();
            builder.Services.AddTransient<IAuthorize, AuthorizationImp>();
            builder.Services.AddTransient<HttpClient>();

            var app = builder.Build();

            app.MapControllers();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

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