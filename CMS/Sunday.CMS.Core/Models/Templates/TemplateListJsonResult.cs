using System.Collections.Generic;
using Sunday.Core.Models.Base;

namespace Sunday.CMS.Core.Models.Templates
{
    public class TemplateListJsonResult : BaseApiResponse
    {
        public int Total { get; set; }
        public List<TemplateItem> Templates { get; set; } = new List<TemplateItem>();
    }
}
