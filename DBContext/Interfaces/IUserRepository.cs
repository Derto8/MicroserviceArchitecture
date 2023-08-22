using DBContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext.Interfaces
{
    public interface IUserRepository: IBaseRepository<Users>
    {
        Task RegistrationAsync(Users user, CancellationToken cancellationToken);
        Task<Users> AuthorizationAsync(string login, string pass, CancellationToken cancellationToken);
    }
}
