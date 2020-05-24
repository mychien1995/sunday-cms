using Sunday.Core;
using Sunday.Core.Domain.Roles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Models.Roles
{
    [MappedTo(typeof(ApplicationRole))]
    public class RoleDetailJsonResult : BaseApiResponse
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string RoleName { get; set; }
    }
}
