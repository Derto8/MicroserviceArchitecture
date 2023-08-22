
using AuthMicroservice;
using AuthMicroservice.AuthClassies;
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

namespace IntraVisionTestTask
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();

            //чтобы из DI-контейнера экшонам контроллера присылался cancellationToken
            builder.Services.AddMvc();

            //настройка параметров валидации jwt
            IConfigurationSection authConfig = builder.Configuration.GetSection("Authorization");
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authConfig["ISSUER"],
                    ValidateAudience = true,
                    ValidAudience = authConfig["AUDIENCE"],
                    ValidateLifetime = true,
                    IssuerSigningKey = KeyEncryption.GetSymmetricSecurityKey(authConfig["KEY"]),
                    ValidateIssuerSigningKey = true,
                };
            }); ;

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
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Events API", Version = "v1" });
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

            app.UseAuthorization();
            app.UseAuthentication();

            
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