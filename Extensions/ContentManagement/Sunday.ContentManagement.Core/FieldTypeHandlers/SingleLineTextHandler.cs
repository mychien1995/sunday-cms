namespace Sunday.ContentManagement.FieldTypeHandlers
{
    public class SingleLineTextHandler : IFieldTypeHandler
    {
        public string Name => "Single-line text";
        public string Layout => "singlelinetext";

        public string? Serialize(object? value) => value?.ToString();

        public object? Deserialize(string? value) => value!;
    }
}
