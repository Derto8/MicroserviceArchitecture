using DBContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext.Interfaces
{
    public interface IUserRepository: IBaseRepository<Users>, IDisposable
    {
        Task Registration(Users user);
        Task<Users> Authorization(string login, string pass);
    }
}
