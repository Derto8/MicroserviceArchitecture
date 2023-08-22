using Shed.CoreKit.WebApi;

namespace AuthMicroservice.Interfaces
{
    public interface IAuthorization
    {
        [HttpPost, Route("authuser/{login}/{password}")]
        Task<IResult> AuthorizationMethod(string login, string password);
    }
}
