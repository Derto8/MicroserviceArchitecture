using DBContext;
using DBContext.Enums;
using DBContext.Interfaces;
using DBContext.Models;
using DBContext.RepositoryServices;
using IntraVisionTestTask.DTOs;
using IntraVisionTestTask.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Data;

namespace IntraVisionTestTask.Controllers
{
    public class DrinksController : Controller
    {
        private ILogger<DrinksController> _logger { get; set; }
        private IDrinksRepository _drinksRepository { get; set; }
        private ICoinsRepository _coinsRepository { get; set; }

        public DrinksController(
            ILogger<DrinksController> logger,
            ILogger<DrinksRepository> loggerRepo,
            ILogger<CoinsRepository> coinRepository,
            ApplicationContext context)
        {
            _logger = logger;
            _drinksRepository = new DrinksRepository(context, loggerRepo);
            _coinsRepository = new CoinsRepository(context, coinRepository);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        //[Authorize(Roles = $"{nameof(RoleEnum.Admin)}")]
        [HttpPost]
        public async Task<IActionResult> Add(string name, decimal price,
            int amount, string img, CancellationToken cancellationToken)
        {
            var drink = new Drinks
            {
                Name = name,
                Price = price,
                Amount = amount,
                Img = img,
            };

            await _drinksRepository.AddAsync(drink, cancellationToken);
            return RedirectToAction("GetAll");
        }

        [HttpGet]
        //[Authorize(Roles = $"{nameof(RoleEnum.Admin)}")]
        public async Task<IActionResult> Update(Guid idDrink, CancellationToken cancellationToken)
        {
            return View(await _drinksRepository.GetAsync(idDrink, cancellationToken));
        }

        [HttpPost]
        //[Authorize(Roles = $"{nameof(RoleEnum.Admin)}")]
        public async Task<IActionResult> Update(Guid idDrink, string name, decimal price, 
            int amount, string img, CancellationToken cancellationToken)
        {
            await _drinksRepository.UpdateAsync(idDrink, new Drinks
            {
                Name = name,
                Price = price,
                Amount = amount,
                Img = img,
            }, cancellationToken);

            return RedirectToAction("GetAll");
        }

        //[Authorize(Roles = $"{nameof(RoleEnum.Admin)}")]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody]Guid idDrink, CancellationToken cancellationToken)
        {
            await _drinksRepository.DeleteAsync(idDrink, cancellationToken);
            return RedirectToAction("GetAll");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var drinks = await _drinksRepository.GetAllAsync(cancellationToken);
            var coins = await _coinsRepository.GetAllAsync(cancellationToken);
            var coindDrinks = new DrinksCoins()
            {
                Coins = coins,
                Drinks = drinks
            };
            return View(coindDrinks);
        }
    }
}
