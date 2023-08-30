using DBContext;
using DBContext.Enums;
using DBContext.Interfaces;
using DBContext.Models;
using DBContext.RepositoryServices;
using IntraVisionTestTask.DTOs;
using IntraVisionTestTask.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntraVisionTestTask.Controllers
{
    public class DrinksController : Controller
    {
        private ILogger<DrinksController> _logger { get; set; }
        private IDrinksRepository _drinksRepository { get; set; }
        private ICoinsRepository _coinsRepository { get; set; }
        private IConfiguration _configuration { get; set; }
        private IWebHostEnvironment _webHostEnvironment { get; set; }

        public DrinksController(
            ILogger<DrinksController> logger,
            ILogger<DrinksRepository> loggerRepo,
            ILogger<CoinsRepository> coinRepository,
            ApplicationContext context,
            IConfiguration conf,
            IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _drinksRepository = new DrinksRepository(context, loggerRepo);
            _coinsRepository = new CoinsRepository(context, coinRepository);
            _configuration = conf;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize(Roles = $"{nameof(RoleEnum.Admin)}")]
        [HttpPost]
        public async Task<JsonResult> AddAdmin(DrinkFromClient drinkFromClient, CancellationToken cancellationToken)
        {
            Drinks drink = new Drinks
            {
                Id = drinkFromClient.Id,
                Name = drinkFromClient.Name,
                Amount = drinkFromClient.Amount,
                Price = drinkFromClient.Price,
                Img = ImageUploadService.SaveImage(drinkFromClient.Img,
                _webHostEnvironment)
            };
            await _drinksRepository.AddAsync(drink, cancellationToken);
            return Json(_configuration["MainServerAddress"]);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid idDrink, CancellationToken cancellationToken)
        {
            return View(await _drinksRepository.GetAsync(idDrink, cancellationToken));
        }

        [Authorize(Roles = $"{nameof(RoleEnum.Admin)}")]
        [HttpPut]
        public async Task<JsonResult> UpdateAdmin(DrinkFromClient drinkFromClient, CancellationToken cancellationToken)
        {
            Drinks drink = new Drinks
            {
                Id = drinkFromClient.Id,
                Name = drinkFromClient.Name,
                Amount = drinkFromClient.Amount,
                Price = drinkFromClient.Price,
                Img = ImageUploadService.SaveImage(drinkFromClient.Img,
                _webHostEnvironment)
            };

            await _drinksRepository.UpdateAsync(drink, cancellationToken);

            return Json(_configuration["MainServerAddress"]);
        }

        [Authorize(Roles = $"{nameof(RoleEnum.Admin)}")]
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
