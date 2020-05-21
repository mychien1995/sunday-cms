using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core.Models.Users
{
    public class GetUserOptions
    {
        public bool FetchRoles { get; set; }
        public bool FetchOrganizations { get; set; }
    }
}
