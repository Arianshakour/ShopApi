using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shop.Application.Services.Interfaces;
using Shop.Domain.Common;
using Shop.Domain.Dtoes.Authentication;
using Shop.Domain.Entities;
using Shop.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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
                DateTime.UtcNow.AddMinutes(15),
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
        public bool Register(RegisterDto req)
        {
            var userExists = _context.Users.Any(x => x.UserName == req.UserName);

            if (userExists)
                return false;

            var user = new User
            {
                UserName = req.UserName,
                Password = PasswordHelper.EncodePasswordMd5(req.Password),
                Name = req.Name,
                Family = req.Family,
                Email = req.Email
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return true;
        }
        private string HashRefreshToken(string token)
        {
            var tokenBytes = Encoding.UTF8.GetBytes(token);
            var hashBytes = SHA256.HashData(tokenBytes);

            return Convert.ToBase64String(hashBytes);
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
        public string SaveRefreshToken(User user)
        {
            var token = GenerateRefreshToken();

            var refreshToken = new RefreshToken
            {
                Token = HashRefreshToken(token),
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                SessionExpiresAt = DateTime.UtcNow.AddDays(30),//baraye ine ke bade 1 mah dobare karbar Login kone va hey ba refresh tamdid nashe
            };

            _context.RefreshTokens.Add(refreshToken);
            _context.SaveChanges();

            return token;
        }
        //ba in method zirie ba accessToken karbar hata age Expire shode bashe etelaat karbar migirim
        //manzooram hamon claimha mese userId,Permission,... inas
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = _configuration["Authentication:Audience"],

                ValidateIssuer = true,
                ValidIssuer = _configuration["Authentication:Issuer"],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(
                        _configuration["Authentication:SecretForKey"])),

                ValidateLifetime = false//baraye inke tarikh enqeza ra dar nazar nagire
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(
                accessToken,
                tokenValidationParameters,
                out var securityToken);

            if (securityToken is not JwtSecurityToken jwtToken ||
                !jwtToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
        //In method ziri dare accToken qabli va RefToken qabli migire
        //id karbar az claim ba method GetPrincipalFromExpiredToken migire
        //chek mikone RefToken male khode Karbar hast ya na
        //ye accToken va ye RefToken jadid doros mikone va RefToken jadid ra ba qabli dar database jaygozin mikone
        public LoginResponseDto Refresh(RefreshTokenRequestDto req)
        {
            var hashedRefreshToken = HashRefreshToken(req.RefreshToken);

            var principal = GetPrincipalFromExpiredToken(req.AccessToken);

            var userId = principal.FindFirst("Id")?.Value;

            if (!int.TryParse(userId, out var id))
                return null;

            var refreshToken = _context.RefreshTokens.FirstOrDefault(x =>
                x.Token == hashedRefreshToken &&
                x.UserId == id &&
                !x.IsRevoked &&
                x.ExpiresAt > DateTime.UtcNow &&
                x.SessionExpiresAt > DateTime.UtcNow);//baresi zaman neshast

            if (refreshToken == null)
                return null;

            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return null;

            var newAccessToken = GenerateToken(user);
            var newRefreshToken = GenerateRefreshToken();

            refreshToken.Token = HashRefreshToken(newRefreshToken);
            var newExpiry = DateTime.UtcNow.AddDays(7);

            refreshToken.ExpiresAt =
                newExpiry < refreshToken.SessionExpiresAt
                    ? newExpiry
                    : refreshToken.SessionExpiresAt;

            _context.SaveChanges();

            return new LoginResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
        public bool Logout(string refreshToken, int userId)
        {
            //inja motabar boodan refresh token feli ra laqv mikone

            var hashedRefreshToken = HashRefreshToken(refreshToken);

            var token = _context.RefreshTokens.FirstOrDefault(x =>
                x.Token == hashedRefreshToken &&
                x.UserId == userId &&
                !x.IsRevoked);

            if (token == null)
                return false;

            token.IsRevoked = true;
            _context.SaveChanges();

            return true;
        }
    }
}
