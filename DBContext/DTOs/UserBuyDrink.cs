using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext.DTOs
{
    /// <summary>
    /// dto для передачи с клиента на сервер баланса юзера
    /// </summary>
    public class UserBuyDrink
    {
        public Guid IdDrink { get; set; }
        public int Balance { get; set; }
    }
}
