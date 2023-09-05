using DBContext;
using DBContext.Interfaces;
using DBContext.RepositoryServices;
using GlobalExceptionHandling.Middlewares;
using IntraVisionTestTask.ConfigureOptions.Microservices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Shed.CoreKit.WebApi;
using System.Security.Cryptography;
using Helpers;
using Helpers.JWTValidate;
using Helpers.JWTValidate.KeyProviders;
using Helpers.JWTValidate.Interfaces;
using IntraVisionTestTask.MicroservicesRequests;

namespace IntraVisionTestTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();

            builder.Services.Configure<AuthOptions>(
                builder.Configuration.GetSection(AuthOptions.Autorization));

            builder.Services.Configure<AuthMicroserviceOptions>(
                builder.Configuration.GetSection("Microservices")
                .GetSection(AuthMicroserviceOptions.Microservice));

            builder.Services.Configure<DrinksCoinsMicroserviceOptions>(
                builder.Configuration.GetSection("Microservices")
                .GetSection(DrinksCoinsMicroserviceOptions.Microservice));

            //��������� �������������� ������
            builder.Services.ConfigureOptions<JwtBearerOptionsConfiguration>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer();

            //����� �� DI-���������� ������� ����������� ���������� cancellationToken
            builder.Services.AddMvc();

            //����������� � ��
            string conn = builder.Configuration.GetConnectionString("ConnectionDataBase");
            builder.Services.AddDbContext<ApplicationContext>(opt =>
            {
                opt.UseSqlServer(conn);
            });

            //���������� ������ �� ����� � �� ������������� ��
            builder.Services.AddControllers().AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            //���������� ����������� � �������
            builder.Services.AddSwaggerDocumentation();

            builder.Services.AddCors();

            builder.Services.AddTransient<ICoinsRepository, CoinsRepository>();
            builder.Services.AddTransient<IDrinksRepository, DrinksRepository>();
            builder.Services.AddTransient<IPublicKeyProvider, PublicKeyProvider>();

            //��������� � ����������� ������
            builder.Services.AddSessionSettings();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerDocumentation();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            // �������� ��������� ����������
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.MapControllers();
            app.UseSession();

            //��������� ������
            app.UseCors(builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            app.Run();
        }
    }
}