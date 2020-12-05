using System.Collections.Generic;
using Sunday.ContentManagement.Models;
using Sunday.Core.Models.Base;

namespace Sunday.CMS.Core.Models.Contents
{
    public class ContextMenuJsonResult : BaseApiResponse
    {
        public List<ContextMenuItem> Items { get; set; } = new List<ContextMenuItem>();
    }
}
