using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Services.Interfaces;
using Shop.Domain.Dtoes.Authentication;

namespace Shop.EndPoint.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthentication _auten;
        public AuthenticationController(IAuthentication auten)
        {
            _auten = auten;
        }
        [HttpPost]
        public IActionResult Authenticate(AuthenticationDto req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = _auten.Validation(req.UserName, req.Password);
            if (user == null)
            {
                return Unauthorized();
            }
            //download package IdentityModel.Token.JWT && Aspnetcore.JWTBearer
            var tokenToReturn = _auten.GenerateToken(user);
            return Ok(tokenToReturn);
        }
    }
}
