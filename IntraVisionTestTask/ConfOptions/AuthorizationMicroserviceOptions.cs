namespace IntraVisionTestTask.ConfOptions
{
    /// <summary>
    /// конфигурация для подключению к микросервису
    /// </summary>
    public class AuthorizationMicroserviceOptions
    {
        public const string Microservice = "MicroserviceAuthorization";

        public string Addres { get; set; } = string.Empty;
    }
}
