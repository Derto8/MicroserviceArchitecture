using DBContext.Enums;
using DBContext.Interfaces;
using DBContext.Models;
using DBContext.RepositoryServices;
using DBContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DrinksCoinsMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoinsController : ControllerBase
    {
        private readonly ILogger<CoinsController> _logger;
        private readonly ICoinsRepository _coinsRepository;

        public CoinsController(
            ILogger<CoinsController> logger,
            ILogger<CoinsRepository> loggerRepo,
            ApplicationContext context)
        {
            _logger = logger;
            _coinsRepository = new CoinsRepository(context, loggerRepo);
        }

        [HttpGet(template: "GetAllCoins")]
        public async Task<IEnumerable<Coins>> GetAllCoins(CancellationToken cancellationToken)
        {
            return await _coinsRepository.GetAllAsync(cancellationToken);
        }

        [HttpPost(template: "GetCoin")]
        public async Task<Coins> GetCoin(Guid coinId, CancellationToken cancellationToken)
        {
            return await _coinsRepository.GetAsync(coinId, cancellationToken);
        }

        [Authorize(Roles = $"{nameof(RoleEnum.Admin)}")]
        [HttpPut(template: "ChangeAmountCoin")]
        public async Task ChangeAmountCoin(Guid coinId, int amount, CancellationToken cancellationToken)
        {
            await _coinsRepository.ChangeAmountCoinAsync(coinId, amount, cancellationToken);
        }

        [Authorize(Roles = $"{nameof(RoleEnum.Admin)}")]
        [HttpPut(template: "ChangeBlockStatusCoin")]
        public async Task ChangeBlockStatusCoin(Guid coinId, bool state, CancellationToken cancellationToken)
        {
            await _coinsRepository.ChangeBlockStatusCoinAsync(coinId, state, cancellationToken);
        }
    }
}
