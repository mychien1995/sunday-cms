using System;

namespace Sunday.CMS.Core.Models.Templates
{
    public class FieldTypeListJsonResult
    {
        public FieldTypeItem[] FieldTypes { get; set; } = Array.Empty<FieldTypeItem>();
    }

    public class FieldTypeItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Layout { get; set; } = string.Empty;
    }
}
