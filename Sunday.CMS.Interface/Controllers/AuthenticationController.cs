using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sunday.CMS.Core.Application.Identity;
using Sunday.CMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sunday.CMS.Interface.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILoginRepository _loginRepository;
        public AuthenticationController(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public async Task<IActionResult> Authenticate([FromBody]LoginInputModel loginInputModel)
        {
            if (!ModelState.IsValid) return BadRequest();
            var result = await _loginRepository.LoginAsync(loginInputModel);
            if (!result.Success)
            {
                return Unauthorized(result);
            }
            return Ok(result);
        }
    }
}
