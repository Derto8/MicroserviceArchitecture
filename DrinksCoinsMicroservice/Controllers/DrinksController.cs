using DBContext;
using DBContext.Enums;
using DBContext.Interfaces;
using DBContext.Models;
using DBContext.RepositoryServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DrinksCoinsMicroservice.Controllers
{
    /// <summary>
    /// ���������� ��� �������������� � �������� Drinks
    /// </summary>
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

        /// <summary>
        /// ��������� ������ � ������� Drinks
        /// </summary>
        /// <param name="drink">������ Drinks</param>
        /// <param name="cancellationToken">����� ������ ������</param>
        [Authorize(Roles = $"{nameof(RoleEnum.Admin)}")]
        [HttpPost, Route("Add")]
        public async Task Add([FromBody]Drinks drink, CancellationToken cancellationToken)
        {
            await _drinksRepository.AddAsync(drink, cancellationToken);
        }

        /// <summary>
        /// ��������� ������ � ������� Drinks
        /// </summary>
        /// <param name="drink">������ Drinks</param>
        /// <param name="cancellationToken">����� ������ ������</param>
        [Authorize(Roles = $"{nameof(RoleEnum.Admin)}")]
        [HttpPut, Route("Update")]
        public async Task Update([FromBody]Drinks drink, CancellationToken cancellationToken)
        {
            await _drinksRepository.UpdateAsync(drink, cancellationToken);
        }

        /// <summary>
        /// ������� ������ � ������� Drinks
        /// </summary>
        /// <param name="idDrink">Id ������</param>
        /// <param name="cancellationToken">����� ������ ������</param>
        [Authorize(Roles = $"{nameof(RoleEnum.Admin)}")]
        [HttpDelete, Route("Delete/{idDrink}")]
        public async Task Delete(Guid idDrink, CancellationToken cancellationToken)
        {
            await _drinksRepository.DeleteAsync(idDrink, cancellationToken);
        }

        /// <summary>
        /// �������� ���� ������ �� ������� Drinks
        /// </summary>
        /// <param name="idDrink">Id ������</param>
        /// <param name="cancellationToken">����� ������ ������</param>
        /// <returns>������ ������ ����� ������ �� ������� Drinks</returns>
        [HttpPost, Route("GetDrink/{idDrink}")]
        public async Task<Drinks> Get(Guid idDrink, CancellationToken cancellationToken)
        {
            return await _drinksRepository.GetAsync(idDrink, cancellationToken);
        }

        /// <summary>
        /// �������� ��� ������ �� ������� Drinks
        /// </summary>
        /// <param name="cancellationToken">����� ������ ������</param>
        /// <returns>������������ ���� ������ ������ Drinks</returns>
        [HttpGet, Route("GetAllDrinks")]
        public async Task<IEnumerable<Drinks>> GetAll(CancellationToken cancellationToken)
        {
            return await _drinksRepository.GetAllAsync(cancellationToken);
        }
    }
}