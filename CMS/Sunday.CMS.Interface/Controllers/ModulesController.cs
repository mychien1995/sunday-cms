using Microsoft.AspNetCore.Mvc;
using Sunday.CMS.Core.Application.FeatureAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sunday.CMS.Interface.Controllers
{
    public class ModulesController : BaseController
    {
        private readonly IApplicationModuleManager _moduleManager;
        public ModulesController(IApplicationModuleManager moduleManager)
        {
            _moduleManager = moduleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _moduleManager.GetModules();
            return Ok(result);
        }
    }
}
