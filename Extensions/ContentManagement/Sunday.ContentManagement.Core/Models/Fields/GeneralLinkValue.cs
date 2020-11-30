using Newtonsoft.Json;

namespace Sunday.ContentManagement.Models.Fields
{
    public class GeneralLinkValue
    {
        public string LinkText { get; set; }
        public string Url { get; set; }
        public string? Target { get; set; }
        public string? Hint { get; set; }

        [JsonConstructor]
        public GeneralLinkValue(string LinkText, string Url, string? Target = null, string? Hint = null)
        {
            this.LinkText = LinkText;
            this.Url = Url;
            this.Target = Target;
            this.Hint = Hint;
        }
    }
}
