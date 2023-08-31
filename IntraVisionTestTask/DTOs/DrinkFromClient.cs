namespace IntraVisionTestTask.DTOs
{
    /// <summary>
    /// dto для приема данных напитка с клиента
    /// </summary>
    public class DrinkFromClient
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }
        public IFormFile Img { get; set; }
    }
}
