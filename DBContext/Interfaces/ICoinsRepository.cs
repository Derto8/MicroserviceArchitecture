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
        Task ChangeBlockStatusCoin(Guid coinId, bool state);
        Task ChangeAmountCoin(Guid coinId, int amount);
    }
}
