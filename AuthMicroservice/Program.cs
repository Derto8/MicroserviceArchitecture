using AuthMicroservice.AuthClassies;
using AuthMicroservice.Interfaces;
using DBContext;
using DBContext.Interfaces;
using GlobalExceptionHandling.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Shed.CoreKit.WebApi;

namespace AuthMicroservice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddCorrelationToken();
            builder.Services.AddCors();

            builder.Services.AddControllers();

            //подключение бд
            string conn = builder.Configuration.GetConnectionString("ConnectionDataBase");
            builder.Services.AddDbContext<ApplicationContext>(opt =>
            {
                opt.UseSqlServer(conn);
            });

            //игнорируем ссылка на циклы и не сериализируем их
            builder.Services.AddControllers().AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });


            builder.Services.AddTransient<IAuthorization, Authorization>();
            //builder.Services.AddTransient<HttpClient>();

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