using DBContext.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext.Interfaces
{
    public interface IBuyDrinkRepository
    {
        Task BuyDrinkAsync(UserBuyDrink userCoins, CancellationToken cancellationToken);
    }
}
