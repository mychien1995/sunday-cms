using Sunday.Core.Models.Base;

namespace Sunday.CMS.Core.Models.Layout
{
    public class GetLayoutResponse : BaseApiResponse
    {
        public string OrganizationName { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string LogoUri { get; set; } = string.Empty;
        public static GetLayoutResponse Empty => new GetLayoutResponse();
    }
}
