using Newtonsoft.Json;
using Sunday.ContentManagement.Models.Fields;
using Sunday.Core.Extensions;

namespace Sunday.ContentManagement.FieldTypeHandlers
{
    public class LinkHandler : IFieldTypeHandler
    {
        public string Name => "General Link";
        public string Layout => "link";
        public string? Serialize(object? value)
        {
            if (value == null) return null;
            var jsonString = JsonConvert.SerializeObject(value);
            return jsonString.TryJsonParse(out GeneralLinkValue _) ? jsonString : null;
        }

        public object? Deserialize(string? value)
        {
            if (value == null) return null;
            return value.TryJsonParse(out GeneralLinkValue link) ? link : null;
        }
    }
}
