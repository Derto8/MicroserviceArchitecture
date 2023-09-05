using DBContext.Models;
using IntraVisionTestTask.ConfigureOptions.Microservices;
using System.Net.Http.Headers;
using System.Net;

namespace IntraVisionTestTask.MicroservicesRequests
{
    public static class CoinsControllerMicroserviceRequests
    {
        public async static Task<IEnumerable<Coins>> GetAllCoins(DrinksCoinsMicroserviceOptions opt, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, $"{opt.Addres}/api/Coins/GetAllCoins");
                var responce = await client.SendAsync(request, cancellationToken);
                return await responce.Content.ReadFromJsonAsync<IEnumerable<Coins>>();
            }
        }

        public async static Task<Coins> GetCoin(Guid idCoin, DrinksCoinsMicroserviceOptions opt, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, $"{opt.Addres}/api/Coins/GetCoin?coinId={idCoin}");
                var responce = await client.SendAsync(request, cancellationToken);

                Coins content = await responce.Content.ReadFromJsonAsync<Coins>();
                return content;
            }
        }

        public async static Task<HttpStatusCode> ChangeAmountCoin(Guid idCoin, int amount, string jwt, 
            DrinksCoinsMicroserviceOptions opt, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            {
                using var request = new HttpRequestMessage(HttpMethod.Put, $"{opt.Addres}/api/Coins/GetCoin?coinId={idCoin}&amount={amount}");
                request.Headers.Authorization = new AuthenticationHeaderValue($"Bearer", jwt);
                var responce = await client.SendAsync(request, cancellationToken);
                return responce.StatusCode;
            }
        }

        public async static Task<HttpStatusCode> ChangeBlockStatusCoin(Guid idCoin, bool state, string jwt,
            DrinksCoinsMicroserviceOptions opt, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            {
                using var request = new HttpRequestMessage(HttpMethod.Put, $"{opt.Addres}/api/Coins/GetCoin?coinId={idCoin}&state={state}");
                request.Headers.Authorization = new AuthenticationHeaderValue($"Bearer", jwt);
                var responce = await client.SendAsync(request, cancellationToken);
                return responce.StatusCode;
            }
        }
    }
}
