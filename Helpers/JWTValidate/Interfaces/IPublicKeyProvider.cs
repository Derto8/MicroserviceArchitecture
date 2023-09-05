using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.JWTValidate.Interfaces
{
    public interface IPublicKeyProvider
    {
        SecurityKey GetKey();
    }
}
