using DBContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DBContext.Interfaces
{
    public interface IUserRepository: IBaseRepository<Users>
    {
        Task<HttpStatusCode> RegistrationAsync(string login, string pass, CancellationToken cancellationToken);
        Task<Users> AuthorizationAsync(string login, string pass, CancellationToken cancellationToken);
        Task<bool> FindUserByLoginAsync(string login, CancellationToken cancellationToken);
    }
}
