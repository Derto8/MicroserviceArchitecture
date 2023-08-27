using Shed.CoreKit.WebApi;
using System.Net;

namespace AuthMicroservice.Interfaces
{
    public interface IAuthorization
    {
        Task<IResult> AuthorizationMethod(string login, string password, CancellationToken cancellationToken);
        Task<HttpStatusCode> RegistrationMethod(string login, string password, CancellationToken cancellationToken);
    }
}
