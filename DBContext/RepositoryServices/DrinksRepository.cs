using DBContext.Interfaces;
using DBContext.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext.RepositoryServices
{
    public class DrinksRepository : IDrinksRepository
    {
        private ApplicationContext _context;
        private ILogger<DrinksRepository> _logger;
        public DrinksRepository(ApplicationContext context,
            ILogger<DrinksRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(Drinks drink, CancellationToken cancellationToken)
        {
            await _context.DrinksTable.AddAsync(drink, cancellationToken);
            await SaveAsync(cancellationToken);
        }

        public async Task UpdateAsync(Drinks drink, CancellationToken cancellationToken)
        {
            _context.Entry(drink).State = EntityState.Modified;
            await SaveAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid idDrink, CancellationToken cancellationToken)
        {
            Drinks drinkChange = await GetAsync(idDrink, cancellationToken);
            _context.DrinksTable.Remove(drinkChange);
            await SaveAsync(cancellationToken);
        }

        public async Task<Drinks> GetAsync(Guid itemId, CancellationToken cancellationToken)
        {
            return await _context.DrinksTable.Where(c => c.Id == itemId).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Drinks>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.DrinksTable.ToListAsync(cancellationToken);
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
