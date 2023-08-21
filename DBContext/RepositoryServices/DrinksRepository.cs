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

        public async Task Add(Drinks drink)
        {
            await _context.DrinksTable.AddAsync(drink);
            await Save();
        }

        public async Task Update(Guid idDrink, Drinks drink)
        {
            Drinks drinkChange = await Get(idDrink);
            _context.Entry(drink).State = EntityState.Modified;
            await Save();
        }

        public async Task Delete(Guid idDrink)
        {
            Drinks drinkChange = await Get(idDrink);
            _context.DrinksTable.Remove(drinkChange);
        }

        public async Task<Drinks> Get(Guid itemId)
        {
            return await _context.DrinksTable.Where(c => c.Id == itemId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Drinks>> GetAll()
        {
            return await _context.DrinksTable.ToListAsync();
        }

        public async Task Save()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionHandler.CatchEx(ex));
            }
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
