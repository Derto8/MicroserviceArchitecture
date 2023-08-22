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

        public async Task AddAsync(Drinks drink)
        {
            await _context.DrinksTable.AddAsync(drink);
            await SaveAsync();
        }

        public async Task UpdateAsync(Guid idDrink, Drinks drink)
        {
            Drinks drinkChange = await GetAsync(idDrink);
            _context.Entry(drink).State = EntityState.Modified;
            await SaveAsync();
        }

        public async Task DeleteAsync(Guid idDrink)
        {
            Drinks drinkChange = await GetAsync(idDrink);
            _context.DrinksTable.Remove(drinkChange);
        }

        public async Task<Drinks> GetAsync(Guid itemId)
        {
            return await _context.DrinksTable.Where(c => c.Id == itemId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Drinks>> GetAllAsync()
        {
            return await _context.DrinksTable.ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
