namespace AuthMicroservice.Authorization
{
    public class AuthOptions
    {
        public const string Autorization = "Authorization";
        public string ISSUER { get; set; } = string.Empty;
        public string AUDIENCE { get; set; } = string.Empty;
        public string PublicKey { get; set; } = string.Empty;
        public string PrivateKey { get; set; } = string.Empty;
    }
}
