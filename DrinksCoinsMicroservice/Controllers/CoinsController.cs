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
    /// <summary>
    /// Контроллер для взаимодействия с таблицей Coins
    /// </summary>
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

        /// <summary>
        /// Получить все записи таблицы Coins
        /// </summary>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        /// <returns>Перечисление всех данных модели Coins</returns>
        [HttpGet, Route("GetCoins")]
        public async Task<IEnumerable<Coins>> GetAllCoins(CancellationToken cancellationToken)
        {
            return await _coinsRepository.GetAllAsync(cancellationToken);
        }

        /// <summary>
        /// Получить одну конкретную запись из таблицы Coins
        /// </summary>
        /// <param name="coinId">Id записи</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        /// <returns>Модель данных Coins</returns>
        [HttpPost, Route("GetCoin/{coinId}")]
        public async Task<Coins> GetCoin(Guid coinId, CancellationToken cancellationToken)
        {
            return await _coinsRepository.GetAsync(coinId, cancellationToken);
        }

        /// <summary>
        /// Изменяет одну у одной записи в таблице Coins,
        /// атрибут amount (количество)
        /// </summary>
        /// <param name="coinId">Id записи</param>
        /// <param name="amount">Количество</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        [Authorize(Roles = $"{nameof(RoleEnum.Admin)}")]
        [HttpPut, Route("ChangeAmountCoin/{coinId}/{amount}")]
        public async Task ChangeAmountCoin(Guid coinId, int amount, CancellationToken cancellationToken)
        {
             await _coinsRepository.ChangeAmountCoinAsync(coinId, amount, cancellationToken);
        }

        /// <summary>
        /// Изменяет одну у одной записи в таблице Coins,
        /// атрибут state (заблокирована монета)
        /// </summary>
        /// <param name="coinId">Id записи</param>
        /// <param name="amount">Состояние монеты</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        [Authorize(Roles = $"{nameof(RoleEnum.Admin)}")]
        [HttpPut, Route("ChangeBlockStatusCoin/{coinId}/{state}")]
        public async Task ChangeBlockStatusCoin(Guid coinId, bool state, CancellationToken cancellationToken)
        {
            await _coinsRepository.ChangeBlockStatusCoinAsync(coinId, state, cancellationToken);
        }
    }
}
