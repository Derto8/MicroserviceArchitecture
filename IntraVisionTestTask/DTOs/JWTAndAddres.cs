namespace IntraVisionTestTask.DTOs
{
    /// <summary>
    /// DTO, для того, чтобы отслать jwt токен на клиент
    /// с редиректом по какому-либо адресу
    /// </summary>
    public class JWTAndAddres
    {
        public string jwt { get; set; }
        public string addres { get; set; }
    }
}
