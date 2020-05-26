using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Models.Organizations
{
    public interface IOrganizationProperties
    {
        string ColorScheme { get; set; }
        public string LogoLink { get; set; }
    }
}
