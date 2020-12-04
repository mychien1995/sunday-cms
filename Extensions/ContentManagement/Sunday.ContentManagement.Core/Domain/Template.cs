using System;
using AutoMapper;
using Sunday.Core.Domain.Interfaces;

namespace Sunday.ContentManagement.Domain
{
    public class Template : IEntity
    {
        public Guid Id { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public Guid[] BaseTemplateIds { get; set; } = Array.Empty<Guid>();
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public bool IsAbstract { get; set; }
        public bool IsPageTemplate { get; set; }
        public string[] InsertOptions { get; set; } = Array.Empty<string>();

        public TemplateField[] Fields { get; set; } = Array.Empty<TemplateField>();
    }
}
