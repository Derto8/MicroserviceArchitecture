using DBContext;
using DBContext.Enums;
using DBContext.Interfaces;
using DBContext.Models;
using DBContext.RepositoryServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace IntraVisionTestTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrinksController : ControllerBase
    {
        private ILogger<DrinksController> _logger { get; set; }
        private IDrinksRepository _drinksRepository { get; set; }

        public DrinksController(
            ILogger<DrinksController> logger,
            ILogger<DrinksRepository> loggerRepo,
            ApplicationContext context)
        {
            _logger = logger;
            _drinksRepository = new DrinksRepository(context, loggerRepo);
        }

        [Authorize(Roles = $"{nameof(RoleEnum.Admin)}")]
        [HttpPost(template: "AddDrink")]
        public async Task Add(string drink)
        {
            await _drinksRepository.AddAsync(null);
        }

        [Authorize(Roles = $"{nameof(RoleEnum.Admin)}")]
        [HttpPut(template: "UpdateDrink")]
        public async Task Update(Guid idDrink, Drinks drink)
        {
            await _drinksRepository.UpdateAsync(idDrink, drink);
        }

        [Authorize(Roles = $"{nameof(RoleEnum.Admin)}")]
        [HttpDelete(template: "DeleteDrink")]
        public async Task Delete(Guid idDrink)
        {
            await _drinksRepository.DeleteAsync(idDrink);
        }
        [HttpPost(template: "GetDrink")]
        public async Task<Drinks> Get(Guid idDrink)
        {
            return await _drinksRepository.GetAsync(idDrink);
        }

        [HttpGet(template: "GetAllDrinks")]
        public async Task<IEnumerable<Drinks>> GetAll()
        {
            return await _drinksRepository.GetAllAsync();
        }
    }
}
