using System.Collections.Generic;
using Sunday.ContentManagement.Models;

namespace Sunday.ContentDelivery.Core.Models
{
    public class RenderingParameters
    {
        public RenderingParameters(Rendering rendering, Dictionary<string, string> parameters, Content? datasource = null)
        {
            Datasource = datasource;
            Rendering = rendering;
            Parameters = parameters;
        }

        public Content? Datasource { get; }
        public Rendering Rendering { get; }
        public Dictionary<string, string> Parameters { get; }
    }
}
