using DBContext.Models;
using IntraVisionTestTask.ConfigureOptions.Microservices;
using System.Net.Http.Headers;
using System.Net;

namespace IntraVisionTestTask.MicroservicesRequests
{
    /// <summary>
    /// Класс содержищий методу для запросов
    /// к микросервису DrinksCoinsMicroservice, к таблице Coins
    /// </summary>
    public static class CoinsControllerMicroserviceRequests
    {
        /// <summary>
        /// Запрос на получение всех записей из таблицы Coins
        /// </summary>
        /// <param name="opt">Параметры адреса микросервиса</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        /// <returns>Перечисление данных модели Coins</returns>
        public async static Task<IEnumerable<Coins>> GetAllCoins(DrinksCoinsMicroserviceOptions opt, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, $"{opt.Addres}/api/Coins/GetCoins");
                var responce = await client.SendAsync(request, cancellationToken);
                return await responce.Content.ReadFromJsonAsync<IEnumerable<Coins>>();
            }
        }

        /// <summary>
        /// Запрос на получение конкретной записи из таблицы Coins
        /// </summary>
        /// <param name="idCoin">Id записи</param>
        /// <param name="opt">Параметры адреса микросервиса</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        /// <returns>Модель данных Coins</returns>
        public async static Task<Coins> GetCoin(Guid idCoin, DrinksCoinsMicroserviceOptions opt, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, $"{opt.Addres}/api/Coins/GetCoin/{idCoin}");
                var responce = await client.SendAsync(request, cancellationToken);

                Coins content = await responce.Content.ReadFromJsonAsync<Coins>();
                return content;
            }
        }

        /// <summary>
        /// Запрос на изменение атрибута amount (количество),
        /// конкретной записи в таблице Coins (только для роли Admin)
        /// </summary>
        /// <param name="idCoin">Id записи</param>
        /// <param name="amount">Количество</param>
        /// <param name="jwt">JWT так как метод имеет атрибут Authorize</param>
        /// <param name="opt">Параметры адреса микросервиса</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        /// <returns>Статус-код выполнения запроса</returns>
        public async static Task<HttpStatusCode> ChangeAmountCoin(Guid idCoin, int amount, string jwt, 
            DrinksCoinsMicroserviceOptions opt, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            {
                using var request = new HttpRequestMessage(HttpMethod.Put, $"{opt.Addres}/api/Coins/ChangeAmountCoin/{idCoin}/{amount}");
                request.Headers.Authorization = new AuthenticationHeaderValue($"Bearer", jwt);
                var responce = await client.SendAsync(request, cancellationToken);
                return responce.StatusCode;
            }
        }

        /// <summary>
        /// Запрос на изменение атрибута isBlocked (состояние монеты),
        /// конкретной записи в таблице Coins (только для роли Admin)
        /// </summary>
        /// <param name="idCoin">Id записи</param>
        /// <param name="state">Состояние монеты</param>
        /// <param name="jwt">JWT так как метод имеет атрибут Authorize</param>
        /// <param name="opt">Параметры адреса микросервиса</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        /// <returns>Статус-код выполнения запроса</returns>
        public async static Task<HttpStatusCode> ChangeBlockStatusCoin(Guid idCoin, bool state, string jwt,
            DrinksCoinsMicroserviceOptions opt, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            {
                using var request = new HttpRequestMessage(HttpMethod.Put, $"{opt.Addres}/api/Coins/ChangeBlockStatusCoin/{idCoin}/{state}");
                request.Headers.Authorization = new AuthenticationHeaderValue($"Bearer", jwt);
                var responce = await client.SendAsync(request, cancellationToken);
                return responce.StatusCode;
            }
        }
    }
}
