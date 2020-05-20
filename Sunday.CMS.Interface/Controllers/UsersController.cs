using Microsoft.AspNetCore.Mvc;
using Sunday.CMS.Core.Application.Users;
using Sunday.CMS.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sunday.CMS.Interface.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IApplicationUserManager _userManager;
        public UsersController(IApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("search")]
        public async Task<IActionResult> GetUsers([FromBody]SearchUserCriteria criteria)
        {
            var result = await _userManager.SearchUsers(criteria);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody]UserMutationModel data)
        {
            var result = await _userManager.CreateUser(data);
            return Ok(result);
        }
    }
}
