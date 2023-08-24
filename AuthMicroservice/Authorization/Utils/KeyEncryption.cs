using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthMicroservice.Authorization.Utils
{
    public static class KeyEncryption
    {
        public static SymmetricSecurityKey GetSymmetricSecurityKey(string KEY) =>
       new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
