using System.Net;

namespace AuthMicroservice.Interfaces
{
    public interface IAuthorize
    {
        Task<IResult> AuthorizationMethod(string login, string password, CancellationToken cancellationToken);
        Task RegistrationMethod(string login, string password, CancellationToken cancellationToken);
    }
}
