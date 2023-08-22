using DBContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext.Interfaces
{
    public interface ICoinsRepository : IBaseRepository<Coins>, IDisposable
    {
        Task ChangeBlockStatusCoinAsync(Guid coinId, bool state);
        Task ChangeAmountCoinAsync(Guid coinId, int amount);
    }
}
