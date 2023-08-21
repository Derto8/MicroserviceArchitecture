using Shed.CoreKit.WebApi;

namespace AuthMicroservice.Interfaces
{
    public interface IAuthorization : IDisposable
    {
        [HttpPost, Route("authuser/{login}/{password}")]
        Task<IResult> AuthorizationMethod(string login, string password);
    }
}
