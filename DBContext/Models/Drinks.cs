using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext.Models
{
    /// <summary>
    /// Модель таблицы Drinks
    /// </summary>
    public class Drinks
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public string Img { get; set; }
    }
}
