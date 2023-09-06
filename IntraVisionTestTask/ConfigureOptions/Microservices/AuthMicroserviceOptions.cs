namespace IntraVisionTestTask.ConfigureOptions.Microservices
{
    /// <summary>
    /// Класс опций адреса для микросервиса AuthMicroservice
    /// </summary>
    public class AuthMicroserviceOptions
    {
        public const string Microservice = "MicroserviceAuthorization";
        public string Addres { get; set; } = string.Empty;
    }
}
