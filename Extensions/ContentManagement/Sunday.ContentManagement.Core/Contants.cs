namespace Sunday.ContentManagement
{
    public enum ContentType
    {
        Organization = 1,
        Website = 2,
        Content = 3
    }

    public enum ContentStatuses
    {
        Draft = 1,
        Published = 2
    }

    public static class RenderingTypes
    {
        public static KeyValue PageRendering = new KeyValue("PageRendering", "Page Rendering");
        public static KeyValue ViewComponent = new KeyValue("ViewComponent", "View Component");

        public readonly struct KeyValue
        {
            public string Code { get; }
            public string DisplayName { get; }

            public KeyValue(string code, string displayName)
            {
                Code = code;
                DisplayName = displayName;
            }
        }
    }
}
