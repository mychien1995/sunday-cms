using System;
using Sunday.Core.Models.Base;

namespace Sunday.CMS.Core.Models.Organizations
{
    public class CreateOrganizationJsonResult : BaseApiResponse
    {
        public Guid OrganizationId { get; set; }
        public CreateOrganizationJsonResult(Guid orgId)
        {
            this.OrganizationId = orgId;
        }
    }
}
