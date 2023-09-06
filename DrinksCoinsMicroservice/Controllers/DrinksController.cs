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
    /// Контроллер для взаимодействия с таблицей Drinks
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
        /// Добавляет запись в таблицу Drinks
        /// </summary>
        /// <param name="drink">Модель Drinks</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        [Authorize(Roles = $"{nameof(RoleEnum.Admin)}")]
        [HttpPost, Route("Add")]
        public async Task Add([FromBody]Drinks drink, CancellationToken cancellationToken)
        {
            await _drinksRepository.AddAsync(drink, cancellationToken);
        }

        /// <summary>
        /// Обновляет запись в таблице Drinks
        /// </summary>
        /// <param name="drink">Модель Drinks</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        [Authorize(Roles = $"{nameof(RoleEnum.Admin)}")]
        [HttpPut, Route("Update")]
        public async Task Update([FromBody]Drinks drink, CancellationToken cancellationToken)
        {
            await _drinksRepository.UpdateAsync(drink, cancellationToken);
        }

        /// <summary>
        /// Удаляет запись в таблице Drinks
        /// </summary>
        /// <param name="idDrink">Id записи</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        [Authorize(Roles = $"{nameof(RoleEnum.Admin)}")]
        [HttpDelete, Route("Delete/{idDrink}")]
        public async Task Delete(Guid idDrink, CancellationToken cancellationToken)
        {
            await _drinksRepository.DeleteAsync(idDrink, cancellationToken);
        }

        /// <summary>
        /// Получаем одну запись из таблицы Drinks
        /// </summary>
        /// <param name="idDrink">Id записи</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        /// <returns>Модель данных одной записи из таблицы Drinks</returns>
        [HttpPost, Route("GetDrink/{idDrink}")]
        public async Task<Drinks> Get(Guid idDrink, CancellationToken cancellationToken)
        {
            return await _drinksRepository.GetAsync(idDrink, cancellationToken);
        }

        /// <summary>
        /// Получаем все записи из таблицы Drinks
        /// </summary>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        /// <returns>Перечисление всех данных модели Drinks</returns>
        [HttpGet, Route("GetAllDrinks")]
        public async Task<IEnumerable<Drinks>> GetAll(CancellationToken cancellationToken)
        {
            return await _drinksRepository.GetAllAsync(cancellationToken);
        }
    }
}