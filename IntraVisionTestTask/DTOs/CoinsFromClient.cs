namespace IntraVisionTestTask.DTOs
{
    /// <summary>
    /// dto, для приему данных монеты с клиента
    /// </summary>
    public class CoinsFromClient
    {
        public Guid Id { get; set; }
        public int Amount { get; set; }
        public bool IsBlocked { get; set; }
    }
}
