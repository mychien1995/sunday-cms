using System;
using System.Threading.Tasks;
using Sunday.CMS.Core.Models.Templates;
using Sunday.ContentManagement.Models;
using Sunday.Core.Models.Base;

namespace Sunday.CMS.Core.Application
{
    public interface IApplicationTemplateManager
    {
        Task<TemplateListJsonResult> Search(TemplateQuery criteria);

        Task<BaseApiResponse> Create(TemplateItem data);

        Task<BaseApiResponse> Update(TemplateItem data);

        Task<TemplateItem> GetById(Guid templateId);

        Task<BaseApiResponse> Delete(Guid templateId);
        FieldTypeListJsonResult GetFieldTypes();
    }
}
