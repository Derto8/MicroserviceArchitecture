﻿using DBContext.Models;
using System.Security.Claims;

namespace AuthMicroservice.AuthClassies
{
    public static class ClaimSettings 
    { 
        public static List<Claim> GetClaims(Users user)
        {
            var claims = new List<Claim> {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
                };

            return claims;
        }
    }
}
