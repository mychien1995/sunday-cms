using System.Linq;
using Newtonsoft.Json;
using Sunday.ContentManagement.Models.Fields;
using Sunday.Core.Extensions;

namespace Sunday.ContentManagement.FieldTypeHandlers
{
    public class RenderingAreaHandler : IFieldTypeHandler
    {
        public string Name => "Rendering Area";
        public string Layout => "rendering-area";
        public string? Serialize(object? value)
        {
            if (value == null) return null;
            var jsonString = JsonConvert.SerializeObject(value);
            return jsonString.TryJsonParse(out RenderingAreaValue renderingAreaValue) && renderingAreaValue.Renderings.Any() ? jsonString : null;
        }

        public object? Deserialize(string? value)
        {
            if (value == null) return null;
            return value.TryJsonParse(out RenderingAreaValue renderingAreaValue) && renderingAreaValue.Renderings.Any() ? renderingAreaValue : null;
        }
    }
}
