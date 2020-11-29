
using System;

namespace Sunday.ContentManagement.FieldTypeHandlers
{
    public interface IFieldTypeHandler
    {
        string Name { get; }
        string Layout { get; }

        string? Serialize(object? value);

        object? Deserialize(string? value);
    }
}
