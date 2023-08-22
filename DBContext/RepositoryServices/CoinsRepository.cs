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

        public async Task<IEnumerable<Coins>> GetAllAsync()
        {
            return await _context.CoinsTable.ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task ChangeAmountCoinAsync(Guid coinId, int amount)
        {
            if(amount >= 0)
            {
                Coins coinChange = await GetAsync(coinId);
                _context.Entry(coinChange).State = EntityState.Modified;

                coinChange.Amount = amount;
                await SaveAsync();
            }
        }

        public async Task ChangeBlockStatusCoinAsync(Guid coinId, bool state)
        {
            Coins coinChange = await GetAsync(coinId);
            _context.Entry(coinChange).State = EntityState.Modified;

            coinChange.IsBlocked = state;
            await SaveAsync();
        }

        public async Task<Coins> GetAsync(Guid itemId)
        {
            return await _context.CoinsTable.Where(c => c.Equals(itemId)).FirstOrDefaultAsync();
        }
    }
}
