namespace Sunday.ContentManagement.FieldTypeHandlers
{
    public class RichTextHandler : IFieldTypeHandler
    {
        public string Name => "Rich Text";
        public string Layout => "richtext";

        public string? Serialize(object? value) => value?.ToString();

        public object? Deserialize(string? value) => value!;
    }
}
