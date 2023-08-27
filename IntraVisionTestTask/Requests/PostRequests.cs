using IntraVisionTestTask.DTOs;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Logging.Console;

namespace IntraVisionTestTask.Requests
{
    public static class PostRequests
    {
        public static async Task<JWT> Authorize(string login, string pass, IConfiguration conf)
        {
            using (var client = new HttpClient())
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, $"{conf["Addres"]}:5001/api/Authorization/authuser/{login}/{pass}");

                using var responce = await client.SendAsync(request);

                if (responce.ReasonPhrase.Equals("OK"))
                {
                    JWT jwt = await responce.Content.ReadFromJsonAsync<JWT>();
                    return jwt;
                }
                return null;
            }
        }

        public static async Task<bool> Registration(string login, string pass, IConfiguration conf)
        {
            using (var client = new HttpClient())
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, $"{conf["Addres"]}:5001/api/Authorization/registration/{login}/{pass}");
                using var responce = await client.SendAsync(request);

                var status = await responce.Content.ReadAsStringAsync();
                if (status == "200")
                    return true;
                return false;
            }
        }

    }
}
