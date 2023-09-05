using Azure.Core;
using DBContext.Models;
using DTOs.AuthDTOs;
using IntraVisionTestTask.ConfigureOptions.Microservices;
using System.Net;
using System.Net.Http.Headers;

namespace IntraVisionTestTask.MicroservicesRequests
{
    public static class DrinksControllerMicroserviceRequest
    {
        public async static Task<HttpStatusCode> AddDrink(Drinks drink, string jwt, DrinksCoinsMicroserviceOptions opt, CancellationToken cancellationToken)
        {
            using(var client = new HttpClient())
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, $"{opt.Addres}/api/Drinks/AddDrink");
                request.Content = JsonContent.Create(drink);
                request.Headers.Authorization = new AuthenticationHeaderValue($"Bearer", jwt);
                var responce =  await client.SendAsync(request, cancellationToken);
                return responce.StatusCode;
            }
        }

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

        public async static Task<Drinks> GetDrink(Guid idDrink, DrinksCoinsMicroserviceOptions opt, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, $"{opt.Addres}/api/Drinks/GetDrink?idDrink={idDrink}");
                var responce = await client.SendAsync(request, cancellationToken);

                Drinks content = await responce.Content.ReadFromJsonAsync<Drinks>();
                return content;
            }
        }

        public async static Task<HttpStatusCode> DeleteDrink(Guid idDrink, string jwt, DrinksCoinsMicroserviceOptions opt, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            {
                using var request = new HttpRequestMessage(HttpMethod.Delete, $"{opt.Addres}/api/Drinks/DeleteDrink?idDrink={idDrink}");
                request.Headers.Authorization = new AuthenticationHeaderValue($"Bearer", jwt);
                var responce = await client.SendAsync(request, cancellationToken);
                return responce.StatusCode;
            }
        }

        public async static Task<HttpStatusCode> UpdateDrink(Guid idDrink, Drinks drink, string jwt, DrinksCoinsMicroserviceOptions opt, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            {
                using var request = new HttpRequestMessage(HttpMethod.Put, $"{opt.Addres}/api/Drinks/UpdateDrink?idDrink={idDrink}");
                request.Content = JsonContent.Create(drink);
                request.Headers.Authorization = new AuthenticationHeaderValue($"Bearer", jwt);
                var responce = await client.SendAsync(request, cancellationToken);
                return responce.StatusCode;
            }
        }
    }
}
