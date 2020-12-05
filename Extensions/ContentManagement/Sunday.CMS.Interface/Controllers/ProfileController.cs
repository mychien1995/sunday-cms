using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Sunday.CMS.Core.Application;
using Sunday.CMS.Core.Models.Users;
using Sunday.Foundation.Context;

namespace Sunday.CMS.Interface.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly IProfileManager _profileManager;
        public ProfileController(IProfileManager profileManager, ISundayContext context) : base(context)
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

        [HttpPut("changeAvatar")]
        public async Task<IActionResult> ChangeAvatar([FromBody] ChangeAvatarModel model)
        {
            var result = await _profileManager.ChangeAvatar(model);
            return Ok(result);
        }
    }
}
