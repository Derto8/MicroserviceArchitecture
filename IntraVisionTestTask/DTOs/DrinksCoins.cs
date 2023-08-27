using DBContext.Models;

namespace IntraVisionTestTask.DTOs
{
    public class DrinksCoins
    {
        public IEnumerable<Drinks> Drinks { get; set; }
        public IEnumerable<Coins> Coins { get; set; }
    }
}
