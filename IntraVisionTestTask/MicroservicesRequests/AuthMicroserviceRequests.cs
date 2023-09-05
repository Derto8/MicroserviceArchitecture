using DTOs.AuthDTOs;
using IntraVisionTestTask.ConfigureOptions.Microservices;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IntraVisionTestTask.MicroservicesRequests
{
    public static class AuthMicroserviceRequests
    {
        public static async Task<JWT> Authorize(string login, string pass, AuthMicroserviceOptions opt, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, $"{opt.Addres}/api/Authorization/authuser/{login}/{pass}");

                using var responce = await client.SendAsync(request, cancellationToken);

                if (responce.ReasonPhrase.Equals("OK"))
                {
                    JWT jwt = await responce.Content.ReadFromJsonAsync<JWT>();
                    return jwt;
                }
                return null;
            }
        }
    }
}
