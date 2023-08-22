using AuthMicroservice.AuthClassies;
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

            //����������� ��
            string conn = builder.Configuration.GetConnectionString("ConnectionDataBase");
            builder.Services.AddDbContext<ApplicationContext>(opt =>
            {
                opt.UseSqlServer(conn);
            });

            builder.Services.AddTransient<IAuthorization, Authorization>();
            builder.Services.AddTransient<HttpClient>();

            var app = builder.Build();


            app.MapControllers();

            app.UseCorrelationToken();

            //�����
            app.UseCors(builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            // ��������� ��������� ������
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            //���������� ���������� � �������� �����
            app.UseWebApiEndpoint<IAuthorization>();

            app.Run();
        }
    }
}