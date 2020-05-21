using Sunday.Core;
using Sunday.Core.Domain.Roles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Models.Roles.JsonResults
{
    public class RoleListJsonResult : ListApiResponse<RoleItem>
    {
        public RoleListJsonResult() : base()
        {
        }
    }

    [MappedTo(typeof(ApplicationRole))]
    public class RoleItem
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string RoleName { get; set; }
    }
}
