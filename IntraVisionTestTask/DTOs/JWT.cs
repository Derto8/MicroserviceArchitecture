namespace IntraVisionTestTask.DTOs
{
    /// <summary>
    /// DTO JWT и Id пользователя, чтобы принимать данные с 
    /// микросервиса авторизации на клиент
    /// </summary>
    public class JWT
    {
        public string access_token { get; set; }
        public Guid userId { get; set; }
    }
}
