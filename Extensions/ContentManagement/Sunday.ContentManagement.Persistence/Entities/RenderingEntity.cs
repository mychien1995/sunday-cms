using System;

namespace Sunday.ContentManagement.Persistence.Entities
{
    public class RenderingEntity
    {
        public Guid Id { get; set; }
        public string RenderingName { get; set; } = string.Empty;
        public string Properties { get; set; } = string.Empty;
        public bool IsPageRendering { get; set; }
        public bool IsRequireDatasource { get; set; }
        public string DatasourceTemplate { get; set; } = string.Empty;
        public string DatasourceLocation { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
    }
}
