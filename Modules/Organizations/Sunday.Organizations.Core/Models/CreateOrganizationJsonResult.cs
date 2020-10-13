using Sunday.Core;
using Sunday.Core.Models.Base;

namespace Sunday.Organizations.Core.Models
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
