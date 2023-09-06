using DBContext.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext.Models
{
    /// <summary>
    /// Модель таблицы Coins
    /// </summary>
    public class Coins
    {
        public Guid Id { get; set; }
        public int Amount { get; set; }
        public bool IsBlocked { get; set; }
        public CoinDenominationsEnum Denomination { get; set; }
    }
}
