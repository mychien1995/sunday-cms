using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.Users;
using Sunday.Foundation.Context;
using Sunday.Foundation.Models;

namespace Sunday.CMS.Interface.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IApplicationUserManager _userManager;
        public UsersController(IApplicationUserManager userManager, ISundayContext context) : base(context)
        {
            _userManager = userManager;
        }

        [HttpPost("search")]
        public async Task<IActionResult> GetUsers([FromBody] UserQuery criteria)
        {
            var result = await _userManager.SearchUsers(criteria);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] UserMutationModel data)
        {
            var result = await _userManager.CreateUser(data);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserMutationModel data)
        {
            var result = await _userManager.UpdateUser(data);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserById([FromQuery] int id)
        {
            var result = await _userManager.GetUserById(id);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromQuery] int id)
        {
            var result = await _userManager.DeleteUser(id);
            return Ok(result);
        }

        [HttpPut("activate")]
        public async Task<IActionResult> ActivateUser([FromQuery] int id)
        {
            var result = await _userManager.ActivateUser(id);
            return Ok(result);
        }

        [HttpPut("deactivate")]
        public async Task<IActionResult> DeactivateUser([FromQuery] int id)
        {
            var result = await _userManager.DeactivateUser(id);
            return Ok(result);
        }

        [HttpPut("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromQuery] int id)
        {
            var result = await _userManager.ResetUserPassword(id);
            return Ok(result);
        }
    }
}
