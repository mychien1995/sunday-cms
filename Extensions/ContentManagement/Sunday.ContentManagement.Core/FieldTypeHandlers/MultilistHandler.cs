using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sunday.ContentManagement.FieldTypeHandlers
{
    public class MultilistHandler : IFieldTypeHandler
    {
        public string Name => "Multilist";
        public string Layout => "multilist";
        public string? Serialize(object? value)
        {
            switch (value)
            {
                case IEnumerable enumerable:
                    {
                        var parts = new List<string>();
                        foreach (var item in enumerable)
                        {
                            if (item != null && Guid.TryParse(item.ToString(), out var id))
                                parts.Add(id.ToString());
                        }

                        return string.Join('|', parts);
                    }
                default:
                    return null;
            }
        }

        public object? Deserialize(string? value)
        => value?.Split('|').Where(part => Guid.TryParse(part, out _)).ToList();
    }
}
