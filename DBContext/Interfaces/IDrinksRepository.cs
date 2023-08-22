using DBContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext.Interfaces
{
    public interface IDrinksRepository : IBaseRepository<Drinks>
    {
        Task AddAsync(Drinks drink, CancellationToken cancellationToken);
        Task UpdateAsync(Guid idDrink, Drinks drink, CancellationToken cancellationToken);
        Task DeleteAsync(Guid idDrink, CancellationToken cancellationToken);
    }
}
