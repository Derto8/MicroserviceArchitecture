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
        Task ChangeCoin(Guid idCoin, int amount, bool isBlocked, CancellationToken cancellationToken);
    }
}
