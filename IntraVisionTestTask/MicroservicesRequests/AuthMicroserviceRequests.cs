using DTOs.AuthDTOs;
using IntraVisionTestTask.ConfigureOptions.Microservices;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;

namespace IntraVisionTestTask.MicroservicesRequests
{
    public static class AuthMicroserviceRequests
    {
        public static async Task<JWT> Authorize(string login, string pass, 
            AuthMicroserviceOptions opt, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, $"{opt.Addres}/api/Authorization/authUser/{login}/{pass}");

                using var responce = await client.SendAsync(request, cancellationToken);

                if (responce.ReasonPhrase.Equals("OK"))
                {
                    JWT jwt = await responce.Content.ReadFromJsonAsync<JWT>();
                    return jwt;
                }
                return null;
            }
        }

        public static async Task<HttpStatusCode> Registration(string login, string pass, 
            AuthMicroserviceOptions opt, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, $"{opt.Addres}/api/Authorization/regUser/{login}/{pass}");

                using var responce = await client.SendAsync(request, cancellationToken);
                return responce.StatusCode;
            }
        }
    }
}
