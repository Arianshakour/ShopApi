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
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthentication auten, ILogger<AuthenticationController> logger)
        {
            _auten = auten;
            _logger = logger;
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
                //in log haro bezar chon khata nakhorde barname chizi log nemishe pas
                //baraye log security khobe
                _logger.LogWarning(
                    "Failed login attempt. Username: {Username}", req.UserName);

                return Unauthorized();
            }
            //download package IdentityModel.Token.JWT && Aspnetcore.JWTBearer
            var tokenToReturn = _auten.GenerateToken(user);
            return Ok(tokenToReturn);
        }
    }
}
