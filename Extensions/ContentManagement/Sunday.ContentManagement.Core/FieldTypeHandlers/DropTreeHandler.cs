using System;

namespace Sunday.ContentManagement.FieldTypeHandlers
{
    public class DropTreeHandler : IFieldTypeHandler
    {
        public string Name => "Drop Tree";
        public string Layout => "droptree";
        public string? Serialize(object? value)
        {
            if (value is string && Guid.TryParse(value.ToString(), out var id)) return id.ToString();
            return null;
        }

        public object? Deserialize(string? value)
        {
            if (value != null && Guid.TryParse(value, out var id)) return id;
            return null;
        }
    }
}
