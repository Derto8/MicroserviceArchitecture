using DBContext;
using DBContext.Enums;
using DBContext.Interfaces;
using DBContext.Models;
using DBContext.RepositoryServices;
using IntraVisionTestTask.ConfigureOptions.Microservices;
using IntraVisionTestTask.MicroservicesRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data;
using System.Net;

namespace IntraVisionTestTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrinksController : ControllerBase
    {
        private ILogger<DrinksController> _logger { get; set; }
        private DrinksCoinsMicroserviceOptions _options { get; set; }

        public DrinksController(
            ILogger<DrinksController> logger,
            IOptions<DrinksCoinsMicroserviceOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        [HttpPost(template: "AddDrink")]
        public async Task<HttpStatusCode> Add(Drinks drink, CancellationToken cancellationToken)
        {
            string jwt = HttpContext.Session.GetString("token");
            return await DrinksControllerMicroserviceRequest.AddDrink(drink, jwt, _options, cancellationToken);
        }

        [HttpPut(template: "UpdateDrink")]
        public async Task<HttpStatusCode> Update(Guid idDrink, Drinks drink, CancellationToken cancellationToken)
        {
            string jwt = HttpContext.Session.GetString("token");
            return await DrinksControllerMicroserviceRequest.UpdateDrink(idDrink, drink, jwt, _options, cancellationToken);
        }

        [HttpDelete(template: "DeleteDrink")]
        public async Task<HttpStatusCode> Delete(Guid idDrink, CancellationToken cancellationToken)
        {
            string jwt = HttpContext.Session.GetString("token");
            return await DrinksControllerMicroserviceRequest.DeleteDrink(idDrink, jwt, _options, cancellationToken);
        }
        [HttpPost(template: "GetDrink")]
        public async Task<Drinks> Get(Guid idDrink, CancellationToken cancellationToken)
        {
            return await DrinksControllerMicroserviceRequest.GetDrink(idDrink, _options, cancellationToken);
        }

        [HttpGet(template: "GetAllDrinks")]
        public async Task<IEnumerable<Drinks>> GetAll(CancellationToken cancellationToken)
        {
            return await DrinksControllerMicroserviceRequest.GetAllDrinks(_options, cancellationToken);
        }
    }
}
