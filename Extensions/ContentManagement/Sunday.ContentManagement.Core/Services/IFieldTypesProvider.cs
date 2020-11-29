using Sunday.ContentManagement.Domain;

namespace Sunday.ContentManagement.Services
{
    public interface IFieldTypesProvider
    {
        TemplateFieldType[] List();

        TemplateFieldType Get(int code);
    }
}
