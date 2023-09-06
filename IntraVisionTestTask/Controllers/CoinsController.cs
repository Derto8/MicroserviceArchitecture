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
using System.Diagnostics;
using System.Net;

namespace IntraVisionTestTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoinsController : ControllerBase
    {
        private readonly ILogger<CoinsController> _logger;
        private readonly ICoinsRepository _coinsRepository;
        private DrinksCoinsMicroserviceOptions _options { get; set; }

        public CoinsController(
            ILogger<CoinsController> logger,
            ILogger<CoinsRepository> loggerRepo,
            ApplicationContext context,
            IOptions<DrinksCoinsMicroserviceOptions> options)
        {
            _logger = logger;
            _coinsRepository = new CoinsRepository(context, loggerRepo);
            _options = options.Value;
        }

        [HttpGet(template: "GetAllCoins")]
        public async Task<IEnumerable<Coins>> GetAllCoins(CancellationToken cancellationToken)
        {
            return await CoinsControllerMicroserviceRequests.GetAllCoins(_options, cancellationToken);
        }

        [HttpPost(template: "GetCoin")]
        public async Task<Coins> GetCoin(Guid coinId, CancellationToken cancellationToken)
        {
            return await CoinsControllerMicroserviceRequests.GetCoin(coinId, _options, cancellationToken);
        }

        [HttpPut(template: "ChangeAmountCoin")]
        public async Task<HttpStatusCode> ChangeAmountCoin(Guid coinId, int amount, CancellationToken cancellationToken)
        {
            string jwt = HttpContext.Session.GetString("token");
            return await CoinsControllerMicroserviceRequests.ChangeAmountCoin(coinId, amount, jwt, _options, cancellationToken);
        }

        [HttpPut(template: "ChangeBlockStatusCoin")]
        public async Task<HttpStatusCode> ChangeBlockStatusCoin(Guid coinId, bool state, CancellationToken cancellationToken)
        {
            string jwt = HttpContext.Session.GetString("token");
            return await CoinsControllerMicroserviceRequests.ChangeBlockStatusCoin(coinId, state, jwt, _options, cancellationToken);
        }
    }
}
