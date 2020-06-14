using Microsoft.AspNetCore.Mvc;
using Sunday.CMS.Core.Filters;

namespace Sunday.CMS.Core.Attributes
{
    public class OrganizationAuthorizedAttribute : TypeFilterAttribute
    {
        public OrganizationAuthorizedAttribute() : base(typeof(OrganizationAuthorizedFilter))
        {
        }
    }
}
