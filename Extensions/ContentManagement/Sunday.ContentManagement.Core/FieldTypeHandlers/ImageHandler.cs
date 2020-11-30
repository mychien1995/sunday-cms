namespace Sunday.ContentManagement.FieldTypeHandlers
{
    public class ImageHandler : IFieldTypeHandler
    {
        public string Name => "Image";
        public string Layout => "image";
        public string? Serialize(object? value) => value?.ToString();

        public object? Deserialize(string? value) => value;
    }
}
