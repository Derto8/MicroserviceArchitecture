using DBContext;
using DBContext.Enums;
using DBContext.Interfaces;
using DBContext.Models;
using DBContext.RepositoryServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IntraVisionTestTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoinsController : ControllerBase, IDisposable
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
        public async Task<IEnumerable<Coins>> GetAllCoins()
        {
            return await _coinsRepository.GetAll();
        }

        [HttpPost(template: "GetCoin")]
        public async Task<Coins> GetCoin(Guid coinId)
        {
            return await _coinsRepository.Get(coinId);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut(template: "ChangeAmountCoin")]
        public async Task ChangeAmountCoin(Guid coinId, int amount)
        {
            await _coinsRepository.ChangeAmountCoin(coinId, amount);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut(template: "ChangeBlockStatusCoin")]
        public async Task ChangeBlockStatusCoin(Guid coinId, bool state)
        {
            await _coinsRepository.ChangeBlockStatusCoin(coinId, state);
        }

        public void Dispose()
        {
            _coinsRepository.Dispose();
        }
    }
}
