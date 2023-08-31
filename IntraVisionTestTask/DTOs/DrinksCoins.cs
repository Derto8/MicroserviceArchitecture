using DBContext.Models;

namespace IntraVisionTestTask.DTOs
{
    /// <summary>
    /// DTO, чтобы отослать перечисления напитков и монет с сервера
    /// на клиент
    /// </summary>
    public class DrinksCoins
    {
        public IEnumerable<Drinks> Drinks { get; set; }
        public IEnumerable<Coins> Coins { get; set; }
    }
}
