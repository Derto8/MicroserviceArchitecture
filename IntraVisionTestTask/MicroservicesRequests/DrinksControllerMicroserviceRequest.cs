using Azure.Core;
using DBContext.Models;
using DTOs.AuthDTOs;
using IntraVisionTestTask.ConfigureOptions.Microservices;
using System.Net;
using System.Net.Http.Headers;

namespace IntraVisionTestTask.MicroservicesRequests
{
    /// <summary>
    /// Класс содержищий методу для запросов
    /// к микросервису DrinksCoinsMicroservice, к таблице Drinks
    /// </summary>
    public static class DrinksControllerMicroserviceRequest
    {
        /// <summary>
        /// Запрос на добавление записи в таблицу Drinks (только для роли Admin)
        /// </summary>
        /// <param name="drink">Модель записи Drinks</param>
        /// <param name="jwt">JWT так как метод имеет атрибут Authorize</param>
        /// <param name="opt">Параметры адреса микросервиса</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        /// <returns>Статус-код выполнения запроса</returns>
        public async static Task<HttpStatusCode> AddDrink(Drinks drink, string jwt, DrinksCoinsMicroserviceOptions opt, CancellationToken cancellationToken)
        {
            using(var client = new HttpClient())
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, $"{opt.Addres}/api/Drinks/Add");
                request.Content = JsonContent.Create(drink);
                request.Headers.Authorization = new AuthenticationHeaderValue($"Bearer", jwt);
                var responce =  await client.SendAsync(request, cancellationToken);
                return responce.StatusCode;
            }
        }

        /// <summary>
        /// Запрос на получение всех записей из таблицы Drinks
        /// </summary>
        /// <param name="opt">Параметры адреса микросервиса</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        /// <returns>Перечисление данных модели Drinks</returns>
        public async static Task<IEnumerable<Drinks>> GetAllDrinks(DrinksCoinsMicroserviceOptions opt, CancellationToken cancellationToken)
        {
            using(var client = new HttpClient())
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, $"{opt.Addres}/api/Drinks/GetAllDrinks");
                var responce = await client.SendAsync(request, cancellationToken);

                IEnumerable<Drinks> content = await responce.Content.ReadFromJsonAsync<IEnumerable<Drinks>>();
                return content;
            }
        }

        /// <summary>
        /// Запрос на получение конкретной записи из таблицы Drinks
        /// </summary>
        /// <param name="idDrink">Id записи</param>
        /// <param name="opt">Параметры адреса микросервиса</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        /// <returns>Модель данных Drinks</returns>
        public async static Task<Drinks> GetDrink(Guid idDrink, DrinksCoinsMicroserviceOptions opt, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, $"{opt.Addres}/api/Drinks/GetDrink/{idDrink}");
                var responce = await client.SendAsync(request, cancellationToken);

                Drinks content = await responce.Content.ReadFromJsonAsync<Drinks>();
                return content;
            }
        }

        /// <summary>
        /// Запрос на удаление записи из таблицы Drinks (только для роли Admin)
        /// </summary>
        /// <param name="idDrink">Id записи</param>
        /// <param name="jwt">JWT так как метод имеет атрибут Authorize</param>
        /// <param name="opt">Параметры адреса микросервиса</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        /// <returns>Статус-код выполнения запроса</returns>
        public async static Task<HttpStatusCode> DeleteDrink(Guid idDrink, string jwt, DrinksCoinsMicroserviceOptions opt, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            {
                using var request = new HttpRequestMessage(HttpMethod.Delete, $"{opt.Addres}/api/Drinks/Delete/{idDrink}");
                request.Headers.Authorization = new AuthenticationHeaderValue($"Bearer", jwt);
                var responce = await client.SendAsync(request, cancellationToken);
                return responce.StatusCode;
            }
        }

        /// <summary>
        /// Запрос на изменение записи в таблице Drinks (только для роли Admin)
        /// </summary>
        /// <param name="drink">Модель записи Drinks</param>
        /// <param name="jwt">JWT так как метод имеет атрибут Authorize</param>
        /// <param name="opt">Параметры адреса микросервиса</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        /// <returns>Статус-код выполнения запроса</returns>
        public async static Task<HttpStatusCode> UpdateDrink(Drinks drink, string jwt, DrinksCoinsMicroserviceOptions opt, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            {
                using var request = new HttpRequestMessage(HttpMethod.Put, $"{opt.Addres}/api/Drinks/Update");
                request.Content = JsonContent.Create(drink);
                request.Headers.Authorization = new AuthenticationHeaderValue($"Bearer", jwt);
                var responce = await client.SendAsync(request, cancellationToken);
                return responce.StatusCode;
            }
        }
    }
}
