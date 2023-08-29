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
        public BuyDrinkController(
            ILogger<BuyDrinkController> logger,
            ILogger<BuyDrinkRepository> loggerRepo,
            ApplicationContext context
            )
        {
            _logger = logger;
            _buyDrinkRepository = new BuyDrinkRepository(context, loggerRepo);
        }

        [HttpPost]
     //   [Authorize(Roles = $"{nameof(RoleEnum.User)}")]
        public async Task BuyDrinkMethod([FromBody]UserBuyDrink userCoins, CancellationToken cancellationToken)
        {
            await _buyDrinkRepository.BuyDrinkAsync(userCoins, cancellationToken);
        }
    }
}
