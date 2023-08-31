using IntraVisionTestTask.ConfOptions;
using IntraVisionTestTask.DTOs;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace IntraVisionTestTask.Requests
{
    public static class PostRequests
    {
        /// <summary>
        /// пост запрос к микросервису авторизации к контроллеру Authorization, методу authuser для получения jwt
        /// </summary>
        /// <param name="login">логин юзера</param>
        /// <param name="pass">пароль юзера</param>
        /// <param name="opt">конфигурация для подключению к микросервису</param>
        /// <returns>JsonWebToken</returns>
        public static async Task<JWT> Authorize(string login, string pass, AuthorizationMicroserviceOptions opt)
        {
            using (var client = new HttpClient())
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, $"{opt.Addres}/api/Authorization/authuser/{login}/{pass}");

                using var responce = await client.SendAsync(request);

                if (responce.ReasonPhrase.Equals("OK"))
                {
                    JWT jwt = await responce.Content.ReadFromJsonAsync<JWT>();
                    return jwt;
                }
                return null;
            }
        }

        /// <summary>
        /// пост запрос к микросервису авторизации к контроллеру Authorization, методу registration
        /// для добавления нового юзера (записи) в бд
        /// </summary>
        /// <param name="login">логин юзера</param>
        /// <param name="pass">пароль юзера</param>
        /// <param name="opt">конфигурация для подключению к микросервису</param>
        /// <returns>результат добавления новой записи в бд</returns>
        public static async Task<bool> Registration(string login, string pass, AuthorizationMicroserviceOptions opt)
        {
            using (var client = new HttpClient())
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, $"{opt.Addres}:5001/api/Authorization/registration/{login}/{pass}");
                using var responce = await client.SendAsync(request);

                var status = await responce.Content.ReadAsStringAsync();
                if (status == "200")
                    return true;
                return false;
            }
        }

    }
}
