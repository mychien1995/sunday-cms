using Microsoft.AspNetCore.Mvc;
using Sunday.CMS.Core.Application.Users;
using Sunday.CMS.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sunday.CMS.Interface.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly IProfileManager _profileManager;
        public ProfileController(IProfileManager profileManager)
        {
            _profileManager = profileManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _profileManager.GetCurrentUserProfile();
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UserMutationModel model)
        {
            var result = await _profileManager.UpdateProfile(model);
            return Ok(result);
        }

        [HttpPut("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            var result = await _profileManager.ChangePassword(model);
            return Ok(result);
        }
    }
}
