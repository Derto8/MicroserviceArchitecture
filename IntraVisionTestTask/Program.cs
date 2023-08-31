
using AuthMicroservice;
using AuthMicroservice.Authorization;
using AuthMicroservice.Authorization.Utils.KeyProviders;
using AuthMicroservice.Authorization.Utils.Services;
using AuthMicroservice.Controllers;
using AuthMicroservice.Interfaces;
using DBContext;
using DBContext.Interfaces;
using DBContext.RepositoryServices;
using GlobalExceptionHandling.Middlewares;
using IntraVisionTestTask.ConfOptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Shed.CoreKit.WebApi;
using System.Security.Cryptography;

namespace IntraVisionTestTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();

            //������������ ������������ �����������
            builder.Services.Configure<AuthorizationMicroserviceOptions>(
                builder.Configuration.GetSection(AuthorizationMicroserviceOptions.Microservice));

            //������������ �����������
            builder.Services.Configure<AuthOptions>(
                builder.Configuration.GetSection(AuthOptions.Autorization));

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
            builder.Services.AddControllersWithViews().AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            //���������� ������
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(opt =>
            {
                opt.Cookie.Name = "Application.Session";
                opt.IdleTimeout = TimeSpan.FromMinutes(5);
                opt.Cookie.IsEssential = true;
            });

            builder.Services.AddCors();

            builder.Services.AddTransient<ICoinsRepository, CoinsRepository>();
            builder.Services.AddTransient<IDrinksRepository, DrinksRepository>();
            builder.Services.AddTransient<IBuyDrinkRepository, BuyDrinkRepository>();
            
            builder.Services.AddSingleton<IPublicKeyProvider, PublicKeyProvider>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Drinks}/{action=GetAll}/{id?}");


            // �������� ���������� ��������� ����������
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.MapControllers();

            //��������� ������
            app.UseCors(builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            //��������������� �� ����������� �� ����� 5001
           // app.UseWebApiRedirect("api/auth", new WebApiEndpoint<IAuthorization>(new Uri($"{address}:5001"))); 

            app.Run();
        }
    }
}