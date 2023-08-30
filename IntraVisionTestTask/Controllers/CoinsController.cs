using DBContext;
using DBContext.Enums;
using DBContext.Interfaces;
using DBContext.Models;
using DBContext.RepositoryServices;
using IntraVisionTestTask.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IntraVisionTestTask.Controllers
{
    public class CoinsController : Controller
    {
        private readonly ILogger<CoinsController> _logger;
        private readonly ICoinsRepository _coinsRepository;
        private IConfiguration _configuration;
        public CoinsController(
            ILogger<CoinsController> logger,
            ILogger<CoinsRepository> loggerRepo,
            ApplicationContext context,
            IConfiguration conf)
        {
            _logger = logger;
            _coinsRepository = new CoinsRepository(context, loggerRepo);
            _configuration = conf;
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid idCoin, CancellationToken cancellationToken)
        {
            return View(await _coinsRepository.GetAsync(idCoin, cancellationToken));
        }

        [Authorize(Roles = $"{nameof(RoleEnum.Admin)}")]
        [HttpPut]
        public async Task<JsonResult> UpdateAdmin([FromBody]CoinsFromClient coins, CancellationToken cancellationToken)
        {
            await _coinsRepository.ChangeCoin(coins.Id, coins.Amount, coins.IsBlocked, cancellationToken);
            return Json(_configuration["MainServerAddress"]);
        }
    }
}
