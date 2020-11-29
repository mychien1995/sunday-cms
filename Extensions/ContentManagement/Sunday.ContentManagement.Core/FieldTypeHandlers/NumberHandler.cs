namespace Sunday.ContentManagement.FieldTypeHandlers
{
    public class NumberHandler : IFieldTypeHandler
    {
        public string Name => "Number";
        public string Layout => "number";
        public string? Serialize(object? value) => value != null ? int.Parse(value.ToString()!).ToString() : null;

        public object? Deserialize(string? value) => value != null ? (int?)int.Parse(value) : null;
    }
}
