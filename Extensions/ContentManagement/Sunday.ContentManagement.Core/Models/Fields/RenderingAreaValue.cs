using System;
using System.Collections.Generic;

namespace Sunday.ContentManagement.Models.Fields
{
    public class RenderingAreaValue
    {
        public RenderingValue[] Renderings { get; set; } = Array.Empty<RenderingValue>();
    }

    public class RenderingValue
    {
        public Guid RenderingId { get; set; }
        public Guid? Datasource { get; set; }
        public Dictionary<string, string> Parameters = new Dictionary<string, string>();
    }
}
