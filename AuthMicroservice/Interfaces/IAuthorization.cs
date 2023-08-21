using Shed.CoreKit.WebApi;

namespace AuthMicroservice.Interfaces
{
    public interface IAuthorization : IDisposable
    {
        [HttpPost, Route("authuser/{login}/{password}")]
        Task<string> AuthorizationMethod(string login, string password);
    }
}
