using DBContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext.Interfaces
{
    /// <summary>
    /// Интерфейс определяющий методы для взаимодействия с таблицей Drinks
    /// </summary>
    public interface IDrinksRepository : IBaseRepository<Drinks>
    {
        /// <summary>
        /// Метод добавления записи в таблицу Drinks
        /// </summary>
        /// <param name="drink">Модель данных записи</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        Task AddAsync(Drinks drink, CancellationToken cancellationToken);

        /// <summary>
        /// Метод обновления записи в таблице Drinks
        /// </summary>
        /// <param name="drink">Модель данных записи</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        Task UpdateAsync(Drinks drink, CancellationToken cancellationToken);

        /// <summary>
        /// Метод удаления записи из таблицы Drinks
        /// </summary>
        /// <param name="idDrink">Id записи</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        Task DeleteAsync(Guid idDrink, CancellationToken cancellationToken);
    }
}
