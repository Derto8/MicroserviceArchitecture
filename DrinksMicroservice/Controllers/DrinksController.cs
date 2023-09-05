using DBContext;
using DBContext.Enums;
using DBContext.Interfaces;
using DBContext.Models;
using DBContext.RepositoryServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrinksMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DrinksController : ControllerBase
    {
        private readonly ILogger<DrinksController> _logger;
        private IDrinksRepository _drinksRepository { get; set; }

        public DrinksController(ILogger<DrinksController> logger,
            ILogger<DrinksRepository> loggerRepo,
            ApplicationContext context)
        {
            _logger = logger;
            _drinksRepository = new DrinksRepository(context, loggerRepo);
        }

        [Authorize(Roles = $"{nameof(RoleEnum.Admin)}")]
        [HttpPost(template: "AddDrink")]
        public async Task Add([FromBody]Drinks drink, CancellationToken cancellationToken)
        {
            await _drinksRepository.AddAsync(drink, cancellationToken);
        }

        [Authorize(Roles = $"{nameof(RoleEnum.Admin)}")]
        [HttpPut(template: "UpdateDrink")]
        public async Task Update(Guid idDrink, Drinks drink, CancellationToken cancellationToken)
        {
            await _drinksRepository.UpdateAsync(idDrink, drink, cancellationToken);
        }

        [Authorize(Roles = $"{nameof(RoleEnum.Admin)}")]
        [HttpDelete(template: "DeleteDrink")]
        public async Task Delete(Guid idDrink, CancellationToken cancellationToken)
        {
            await _drinksRepository.DeleteAsync(idDrink, cancellationToken);
        }
        [HttpPost(template: "GetDrink")]
        public async Task<Drinks> Get(Guid idDrink, CancellationToken cancellationToken)
        {
            return await _drinksRepository.GetAsync(idDrink, cancellationToken);
        }

        [HttpGet(template: "GetAllDrinks")]
        public async Task<IEnumerable<Drinks>> GetAll(CancellationToken cancellationToken)
        {
            return await _drinksRepository.GetAllAsync(cancellationToken);
        }
    }
}