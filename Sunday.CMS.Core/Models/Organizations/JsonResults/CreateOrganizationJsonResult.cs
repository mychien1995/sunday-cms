using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.CMS.Core.Models.Organizations
{
    public class CreateOrganizationJsonResult : BaseApiResponse
    {
        public int OrganizationId { get; set; }
        public CreateOrganizationJsonResult() : base()
        {

        }
        public CreateOrganizationJsonResult(int orgId) : base()
        {
            this.OrganizationId = orgId;
        }
    }
}
