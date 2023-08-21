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
    public class UserRepository : IUserRepository
    {
        private ApplicationContext _context;
        private ILogger<UserRepository> _logger;

        public UserRepository(ApplicationContext context,
            ILogger<UserRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Users> Authorization(string login, string pass)
        {
            Users? user = await _context.UsersTable.Where(c => c.Login == login
                && c.Password == pass).FirstOrDefaultAsync();
            return user;
        }

        public async Task<Users> Get(Guid itemId)
        {
            return await _context.UsersTable.Where(c => c.Id == itemId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Users>> GetAll()
        {
            return await _context.UsersTable.ToListAsync();
        }

        public async Task Registration(Users user)
        {
            await _context.UsersTable.AddAsync(user);
            await Save();
        }

        public async Task Save()
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
