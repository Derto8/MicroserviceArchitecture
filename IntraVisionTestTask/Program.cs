
using AuthMicroservice;
using AuthMicroservice.Authorization;
using AuthMicroservice.Authorization.Utils.Services;
using AuthMicroservice.Controllers;
using AuthMicroservice.Interfaces;
using DBContext;
using DBContext.Interfaces;
using DBContext.RepositoryServices;
using GlobalExceptionHandling.Middlewares;
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

            builder.Services.Configure<AuthOptions>(
                builder.Configuration.GetSection(AuthOptions.Autorization));

            builder.Services.ConfigureOptions<JwtBearerOptionsConfiguration>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer();

            //чтобы из DI-контейнера экшонам контроллера присылался cancellationToken
            builder.Services.AddMvc();

            //подключение к бд
            string conn = builder.Configuration.GetConnectionString("ConnectionDataBase");
            builder.Services.AddDbContext<ApplicationContext>(opt =>
            {
                opt.UseSqlServer(conn);
            });

            //игнорируем ссылка на циклы и не сериализируем их
            builder.Services.AddControllers().AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            //добавление авторизации в сваггер
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Drinks API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            builder.Services.AddCors();

            builder.Services.AddTransient<ICoinsRepository, CoinsRepository>();
            builder.Services.AddTransient<IDrinksRepository, DrinksRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            // мидлварь обработки исключений
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.MapControllers();

            //настройка корсов
            app.UseCors(builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            string address = builder.Configuration["Addres"];

            //перенаправление на микросервис по порту 5001
            app.UseWebApiRedirect("api/auth", new WebApiEndpoint<IAuthorization>(new Uri($"{address}:5001"))); 

            app.Run();
        }
    }
}