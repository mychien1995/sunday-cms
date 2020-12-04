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

    public enum FieldTypes
    {
        SingleLineText = 1,
        MultilineText = 2,
        Number = 3,
        RichText = 4,
        Image = 5,
        Link = 6,
        RenderingArea = 7,
        DropTree = 8,
        Multilist = 9
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
