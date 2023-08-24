namespace AuthMicroservice.Authorization
{
    public class AuthOptions
    {
        public const string Autorization = "Authorization";
        public string ISSUER { get; set; } = string.Empty;
        public string AUDIENCE { get; set; } = string.Empty;
        public string KEY { get; set; } = string.Empty;
    }
}
