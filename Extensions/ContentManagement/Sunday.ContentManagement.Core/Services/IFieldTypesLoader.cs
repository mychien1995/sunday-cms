using Sunday.ContentManagement.Domain;

namespace Sunday.ContentManagement.Services
{
    public interface IFieldTypesLoader
    {
        TemplateFieldType[] List();

        TemplateFieldType Get(int code);
    }
}
