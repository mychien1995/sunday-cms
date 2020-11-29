namespace Sunday.ContentManagement.FieldTypeHandlers
{
    public class MultilineTextHandler : IFieldTypeHandler
    {
        public string Name => "Multiline Text";
        public string Layout => "multiline-text";

        public string? Serialize(object? value) => value?.ToString();

        public object? Deserialize(string? value) => value!;
    }
}
