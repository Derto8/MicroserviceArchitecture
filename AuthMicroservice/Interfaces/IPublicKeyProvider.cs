using Microsoft.IdentityModel.Tokens;

namespace AuthMicroservice.Interfaces
{
    public interface IPublicKeyProvider
    {
        SecurityKey GetKey();
    }
}
