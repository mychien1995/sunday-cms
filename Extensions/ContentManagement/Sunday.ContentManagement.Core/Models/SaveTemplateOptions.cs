namespace Sunday.ContentManagement.Models
{
    public class SaveTemplateOptions
    {
        public bool SaveProperties { get; set; }
        public static SaveTemplateOptions Default = new SaveTemplateOptions();
    }
}
