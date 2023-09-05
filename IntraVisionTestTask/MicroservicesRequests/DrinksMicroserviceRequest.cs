using Azure.Core;
using DBContext.Models;
using IntraVisionTestTask.ConfigureOptions.Microservices;
using System.Net.Http.Headers;

namespace IntraVisionTestTask.MicroservicesRequests
{
    public static class DrinksMicroserviceRequest
    {
        public async static Task AddDrink(Drinks drink, DrinksMicroserviceOptions opt, string jwt)
        {
            using(var client = new HttpClient())
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, $"{opt.Addres}/api/Drinks/AddDrink");
                request.Content = JsonContent.Create(drink);
                request.Headers.Authorization = new AuthenticationHeaderValue($"Bearer", jwt);
                await client.SendAsync(request);
            }
        }
    }
}
