using System;
using System.Collections.Generic;
using Sunday.Core.Domain.Interfaces;

namespace Sunday.ContentManagement.Models
{
    public class Rendering : IEntity
    {
        public Guid Id { get; set; }
        public string RenderingName { get; set; } = string.Empty;
        public string RenderingType { get; set; } = string.Empty;

        public Dictionary<string, string> Properties { get; set; }= new Dictionary<string, string>();
        public bool IsPageRendering { get; set; }
        public bool IsRequireDatasource { get; set; }
        public Guid? DatasourceTemplate { get; set; }
        public string DatasourceLocation { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
    }
}
