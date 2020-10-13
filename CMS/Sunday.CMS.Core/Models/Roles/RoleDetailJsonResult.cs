﻿using System;
using Sunday.Core;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Models.Roles
{
    [MappedTo(typeof(ApplicationRole))]
    public class RoleDetailJsonResult : BaseApiResponse
    {
        public RoleDetailJsonResult(Guid id, string code, string roleName, bool requireOrganization)
        {
            Id = id;
            Code = code;
            RoleName = roleName;
            RequireOrganization = requireOrganization;
        }

        public Guid Id { get; set; }
        public string Code { get; set; }
        public string RoleName { get; set; }

        public bool RequireOrganization { get; set; }

    }
}
