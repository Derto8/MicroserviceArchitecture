using DBContext.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext.Models
{
    /// <summary>
    /// Модель таблицы Users
    /// </summary>
    public class Users
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public RoleEnum Role { get; set; }
    }
}
