using DBContext.Interfaces;
using DBContext.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DBContext.RepositoryServices
{
    public class CoinsRepository : ICoinsRepository
    {
        private ApplicationContext _context;
        private ILogger<CoinsRepository> _logger;

        public CoinsRepository(ApplicationContext context, ILogger<CoinsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Coins>> GetAll()
        {
            _logger.LogError($"Error in {typeof(CoinsRepository)}");
            return await _context.CoinsTable.ToListAsync();
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

        public async Task ChangeAmountCoin(Guid coinId, int amount)
        {
            if(amount >= 0)
            {
                Coins coinChange = await Get(coinId);
                _context.Entry(coinChange).State = EntityState.Modified;

                coinChange.Amount = amount;
                await Save();
            }
        }

        public async Task ChangeBlockStatusCoin(Guid coinId, bool state)
        {
            Coins coinChange = await Get(coinId);
            _context.Entry(coinChange).State = EntityState.Modified;

            coinChange.IsBlocked = state;
            await Save();
        }

        public async Task<Coins> Get(Guid itemId)
        {
            try
            {
                return await _context.CoinsTable.Where(c => c.Equals(itemId)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionHandler.CatchEx(ex));
                return null;
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
