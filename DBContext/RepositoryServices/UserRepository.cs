using DBContext.Interfaces;
using DBContext.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public async Task<Users> AuthorizationAsync(string login, string pass, CancellationToken cancellationToken)
        {
            Users? user = await _context.UsersTable.Where(c => c.Login == login
                && c.Password == pass).FirstOrDefaultAsync(cancellationToken);
            return user;
        }

        public async Task<Users> GetAsync(Guid itemId, CancellationToken cancellationToken)
        {
            return await _context.UsersTable.Where(c => c.Id == itemId).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Users>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.UsersTable.ToListAsync(cancellationToken);
        }

        public async Task<HttpStatusCode> RegistrationAsync(string login, string pass, CancellationToken cancellationToken)
        {
            if(await FindUserByLoginAsync(login, cancellationToken))
            {
                Users user = new Users()
                {
                    Id = Guid.NewGuid(),
                    Login = login,
                    Password = pass,
                    Role = Enums.RoleEnum.User
                };

                await _context.UsersTable.AddAsync(user, cancellationToken);
                await SaveAsync(cancellationToken);
                _logger.LogInformation($"{login} - зарегистрировался");
                return HttpStatusCode.OK;
            }
            return HttpStatusCode.Conflict;
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> FindUserByLoginAsync(string login, CancellationToken cancellationToken)
        {
            Users user = await _context.UsersTable.Where(c => c.Login == login).FirstOrDefaultAsync(cancellationToken);
            if (user == null)
                return true;
            return false;
        }
    }
}
