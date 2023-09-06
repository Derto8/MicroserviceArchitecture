using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext.Interfaces
{
    /// <summary>
    /// интерфейс определяющий методы интерфейсов, имплементирующих его
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseRepository<T> where T : class
    {
        /// <summary>
        /// Получить все данный из таблицы БД
        /// </summary>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        /// <returns>Перечисление данных в моделях T</returns>
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
        /// <summary>
        /// Получить одну запись из таблицы БД
        /// </summary>
        /// <param name="itemId">Id-записи</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        /// <returns>Модель данных</returns>
        Task<T> GetAsync(Guid itemId, CancellationToken cancellationToken);
        /// <summary>
        /// Сохраняет состояние БД
        /// </summary>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        Task SaveAsync(CancellationToken cancellationToken);
    }
}
