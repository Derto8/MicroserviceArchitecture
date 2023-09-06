using DTOs.AuthDTOs;
using IntraVisionTestTask.ConfigureOptions.Microservices;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;

namespace IntraVisionTestTask.MicroservicesRequests
{
    /// <summary>
    /// Класс содержащий методы запросов к микросервису AuthMicroservice
    /// </summary>
    public static class AuthMicroserviceRequests
    {
        /// <summary>
        /// Запрос на авторизацию юзера
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="pass">Пароль</param>
        /// <param name="opt">Параметры адреса микросервиса</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        /// <returns>JSON, в котором находится jwt и id записи с данными юзера, 
        /// которому принадлежит данный jwt</returns>
        public static async Task<JWT> Authorize(string login, string pass, 
            AuthMicroserviceOptions opt, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, $"{opt.Addres}/api/Authorization/AuthUser/{login}/{pass}");

                using var responce = await client.SendAsync(request, cancellationToken);

                if (responce.ReasonPhrase.Equals("OK"))
                {
                    JWT jwt = await responce.Content.ReadFromJsonAsync<JWT>();
                    return jwt;
                }
                return null;
            }
        }

        /// <summary>
        /// Запрос на регистрацию юзера
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="pass">Пароль</param>
        /// <param name="opt">Параметры адреса микросервиса</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        /// <returns>Статус-код выполнения запроса</returns>
        public static async Task<HttpStatusCode> Registration(string login, string pass, 
            AuthMicroserviceOptions opt, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, $"{opt.Addres}/api/Authorization/RegUser/{login}/{pass}");

                using var responce = await client.SendAsync(request, cancellationToken);
                return responce.StatusCode;
            }
        }
    }
}
