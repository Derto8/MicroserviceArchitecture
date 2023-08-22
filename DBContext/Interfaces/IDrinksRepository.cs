using DBContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext.Interfaces
{
    public interface IDrinksRepository : IBaseRepository<Drinks>, IDisposable
    {
        Task AddAsync(Drinks drink);
        Task UpdateAsync(Guid idDrink, Drinks drink);
        Task DeleteAsync(Guid idDrink);
    }
}
