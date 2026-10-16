using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shop.Application.Services.Interfaces;
using Shop.Domain.Dtoes.Authentication;

namespace Shop.EndPoint.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthentication _auten;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthentication auten, ILogger<AuthenticationController> logger)
        {
            _auten = auten;
            _logger = logger;
        }
        private int? GetCurrentUserId()
        {
            //in method userId karbar feli ra barmigardoone
            //bayad inja bashe nemishe bord to service chon using httpContext dare
            //va nabayad be laye service vabaste beshe
            var userId = User.FindFirst("Id")?.Value;

            if (string.IsNullOrEmpty(userId))
                return null;

            if (!int.TryParse(userId, out var id))
                return null;

            return id;
        }
        [HttpPost("Login")]
        public IActionResult Login(AuthenticationDto req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = _auten.Validation(req.UserName, req.Password);
            if (user == null)
            {
                //in log haro bezar chon khata nakhorde barname chizi log nemishe pas
                //baraye log security khobe
                _logger.LogWarning(
                    "Failed login attempt. Username: {Username}", req.UserName);

                return Unauthorized();
            }
            //download package IdentityModel.Token.JWT && Aspnetcore.JWTBearer
            var accessToken = _auten.GenerateToken(user);
            var refreshToken = _auten.SaveRefreshToken(user);

            var response = new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return Ok(response);
        }
        [HttpPost("Register")]
        public IActionResult Register(RegisterDto req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _auten.Register(req);

            if (!result)
                return Conflict("Username already exists.");

            return Ok("User registered successfully.");
        }
        [HttpPost("refresh")]
        public IActionResult Refresh(RefreshTokenRequestDto req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var response = _auten.Refresh(req);

                if (response == null)
                    return Unauthorized("Invalid or expired refresh token.");

                return Ok(response);
            }
            catch (SecurityTokenException)
            {
                return Unauthorized("Invalid access token.");
            }
            catch (ArgumentException)
            {
                return Unauthorized("Invalid access token.");
            }
        }
        //Hatman [Authorize] baraye Logout bezar ta harkasi natoone Request bezane
        //Hatman faqat karbari ke ehraz hoviat shode bayad betoone
        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout(LogoutRequestDto req)
        {
            var userId = GetCurrentUserId();

            if (userId == null)
                return Unauthorized();

            var result = _auten.Logout(req.RefreshToken, userId.Value);

            if (!result)
                return BadRequest("Invalid or already revoked refresh token.");

            return Ok("Logged out successfully.");
        }
    }
}
