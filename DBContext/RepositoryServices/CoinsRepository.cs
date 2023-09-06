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

        public async Task<IEnumerable<Coins>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.CoinsTable.ToListAsync(cancellationToken);
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task ChangeAmountCoinAsync(Guid coinId, int amount, CancellationToken cancellationToken)
        {
            if(amount >= 0)
            {
                Coins coinChange = await GetAsync(coinId, cancellationToken);
                _context.Entry(coinChange).State = EntityState.Modified;

                coinChange.Amount = amount;
                await SaveAsync(cancellationToken);
            }
        }

        public async Task ChangeBlockStatusCoinAsync(Guid coinId, bool state, CancellationToken cancellationToken)
        {
            Coins coinChange = await GetAsync(coinId, cancellationToken);
            _context.Entry(coinChange).State = EntityState.Modified;

            coinChange.IsBlocked = state;
            await SaveAsync(cancellationToken);
        }

        public async Task<Coins> GetAsync(Guid itemId, CancellationToken cancellationToken)
        {
            return await _context.CoinsTable.Where(c => c.Id.Equals(itemId)).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
