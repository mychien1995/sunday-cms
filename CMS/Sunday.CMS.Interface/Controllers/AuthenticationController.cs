using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sunday.CMS.Core.Models;
using System.Threading.Tasks;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.Login;

namespace Sunday.CMS.Interface.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILoginManager _loginManager;
        public AuthenticationController(ILoginManager loginManager)
        {
            _loginManager = loginManager;
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public async Task<IActionResult> Authenticate([FromBody] LoginInputModel loginInputModel)
        {
            if (!ModelState.IsValid) return BadRequest();
            var result = await _loginManager.LoginAsync(loginInputModel);
            if (!result.Success)
            {
                return Unauthorized(result);
            }
            return Ok(result);
        }
    }
}
