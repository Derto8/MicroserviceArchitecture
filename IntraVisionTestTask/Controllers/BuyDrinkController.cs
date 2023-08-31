using DBContext;
using DBContext.DTOs;
using DBContext.Enums;
using DBContext.Interfaces;
using DBContext.RepositoryServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace IntraVisionTestTask.Controllers
{
    public class BuyDrinkController : Controller
    {
        private ILogger<BuyDrinkController> _logger;
        private IBuyDrinkRepository _buyDrinkRepository;
        private IConfiguration _configuration;
        public BuyDrinkController(
            ILogger<BuyDrinkController> logger,
            ILogger<BuyDrinkRepository> loggerRepo,
            ApplicationContext context,
            IConfiguration conf
            )
        {
            _logger = logger;
            _buyDrinkRepository = new BuyDrinkRepository(context, loggerRepo);
            _configuration = conf;
        }

        /// <summary>
        /// Метод покупки юзером напитка
        /// </summary>
        /// <param name="userCoins">Модель данных юзера необходимых для покупки
        /// напитка, полученная с клиента</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        /// <returns>Страница с формой обновления данных монеты</returns>
        [HttpPost]
        [Authorize(Roles = $"{nameof(RoleEnum.User)}")]
        public async Task<JsonResult> BuyDrinkMethod([FromBody]UserBuyDrink userCoins, CancellationToken cancellationToken)
        {
            await _buyDrinkRepository.BuyDrinkAsync(userCoins, cancellationToken);
            return Json(_configuration["MainServerAddress"]);
        }
    }
}
