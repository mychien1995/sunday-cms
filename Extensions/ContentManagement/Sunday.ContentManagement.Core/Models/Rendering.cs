using System;
using Sunday.Core.Domain.Interfaces;

namespace Sunday.ContentManagement.Models
{
    public class Rendering : IEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public Guid Id { get; set; }
    }
}
