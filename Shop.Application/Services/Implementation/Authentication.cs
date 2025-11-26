using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shop.Application.Services.Interfaces;
using Shop.Domain.Common;
using Shop.Domain.Entities;
using Shop.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Services.Implementation
{
    public class Authentication : IAuthentication
    {
        private readonly IConfiguration _configuration;
        //baraye dastresi be appsettings
        private readonly ShopContext _context;

        public Authentication(IConfiguration configuration, ShopContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public User Validation(string username, string pass)
        {
            var passToHash = PasswordHelper.EncodePasswordMd5(pass);
            var user = _context.Users.FirstOrDefault(x => x.UserName == username && x.Password == passToHash);
            return user;
        }


        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes
                (_configuration["Authentication:SecretForKey"]));
            var signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256
                );
            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("Id", user.Id.ToString()));
            claimsForToken.Add(new Claim("UserName", user.UserName.ToString()));

            // دریافت پرمیژن‌ها از دیتابیس
            var permissions = getPermission(user);

            foreach (var perm in permissions)
            {
                claimsForToken.Add(new Claim("permission", perm)); // کلایم اختصاصی پرمیژن
            }

            var jwtSecurityToke = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                //modat zaman motabar boodan token
                signingCredentials
                );

            var tokenToReturn = new JwtSecurityTokenHandler()
                .WriteToken(jwtSecurityToke);
            return tokenToReturn;
        }

        public List<string> getPermission(User user)
        {
            var permissions = _context.UserRoles.Where(ur => ur.UserId == user.Id)
                .SelectMany(ur => ur.role.rolePermissions)
                .Select(rp => rp.permission.PermissionKey).Distinct().ToList();
            return permissions;
        }

    }
}
