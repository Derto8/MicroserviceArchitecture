
using DBContext;
using Microsoft.EntityFrameworkCore;
using Helpers;
using Helpers.JWTValidate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DBContext.Interfaces;
using DBContext.RepositoryServices;
using Helpers.JWTValidate.Interfaces;
using Helpers.JWTValidate.KeyProviders;
using GlobalExceptionHandling.Middlewares;

namespace DrinksCoinsMicroservice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();

            //чтобы из DI-контейнера экшонам контроллера присылалс€ cancellationToken
            builder.Services.AddMvc();

            //добавл€ю корсы
            builder.Services.AddCors();

            //подключение к бд
            string conn = builder.Configuration.GetConnectionString("ConnectionDataBase");
            builder.Services.AddDbContext<ApplicationContext>(opt =>
            {
                opt.UseSqlServer(conn);
            });

            builder.Services.Configure<AuthOptions>(
                builder.Configuration.GetSection(AuthOptions.Autorization));

            //настройки аутентификации токена
            builder.Services.ConfigureOptions<JwtBearerOptionsConfiguration>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer();

            //игнорируем ссылки на циклы и не сериализируем их
            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            builder.Services.AddSwaggerDocumentation();

            builder.Services.AddTransient<ICoinsRepository, CoinsRepository>();
            builder.Services.AddTransient<IDrinksRepository, DrinksRepository>();
            builder.Services.AddTransient<IPublicKeyProvider, PublicKeyProvider>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerDocumentation();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            //настройка корсов
            app.UseCors(builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.Run();
        }
    }
}