using DBContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext.Interfaces
{
    /// <summary>
    /// Интерфейс определяющий методы для взаимодействия с таблицей Coins
    /// </summary>
    public interface ICoinsRepository : IBaseRepository<Coins>
    {
        /// <summary>
        /// Изменяет атритуб IsBlocked конкретной записи в таблице Coins
        /// </summary>
        /// <param name="coinId">Id-записи</param>
        /// <param name="state">Статус монеты</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        Task ChangeBlockStatusCoinAsync(Guid coinId, bool state, CancellationToken cancellationToken);
        /// <summary>
        /// Изменяет атритуб Amount конкретной записи в таблице Coins
        /// </summary>
        /// <param name="coinId">Id-записи</param>
        /// <param name="amount">Количество монет</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        Task ChangeAmountCoinAsync(Guid coinId, int amount, CancellationToken cancellationToken);
    }
}
