using DBContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext.Interfaces
{
    public interface ICoinsRepository : IBaseRepository<Coins>
    {
        Task ChangeBlockStatusCoinAsync(Guid coinId, bool state, CancellationToken cancellationToken);
        Task ChangeAmountCoinAsync(Guid coinId, int amount, CancellationToken cancellationToken);
    }
}
