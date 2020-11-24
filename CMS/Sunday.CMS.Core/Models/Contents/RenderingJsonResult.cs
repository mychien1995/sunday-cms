using System;
using Sunday.ContentManagement.Models;
using Sunday.Core;
using Sunday.Core.Models.Base;
using Sunday.Foundation.Domain;

namespace Sunday.CMS.Core.Models.Contents
{
    [MappedTo(typeof(Rendering))]
    public class RenderingJsonResult : BaseApiResponse
    {
        public Guid? Id { get; set; }
        public string RenderingName { get; set; } = string.Empty;
        public string Controller { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public bool IsPageRendering { get; set; }
        public bool IsRequireDatasource { get; set; }
        public Guid? DatasourceTemplate { get; set; }
        public string DatasourceLocation { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;

        public EntityAccess? Access { get; set; }
    }
}
