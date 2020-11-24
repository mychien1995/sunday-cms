using System;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Persistence.Application;
using Sunday.ContentManagement.Persistence.Entities;
using Sunday.ContentManagement.Persistence.Implementation.DapperParameters;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.Core.Models.Base;
using Sunday.DataAccess.SqlServer.Database;
using static LanguageExt.Prelude;

namespace Sunday.ContentManagement.Persistence.Implementation.Repositories
{
    [ServiceTypeOf(typeof(ITemplateRepository))]
    public class DefaultTemplateRepository : ITemplateRepository
    {
        private readonly StoredProcedureRunner _dbRunner;
        public DefaultTemplateRepository(StoredProcedureRunner dbRunner)
        {
            _dbRunner = dbRunner;
        }

        public Task<SearchResult<TemplateEntity>> QueryAsync(TemplateQuery query)
            => _dbRunner.ExecuteMultipleAsync<int, TemplateEntity>(ProcedureNames.Templates.Search, query).MapResultTo(
                rs =>
                    new SearchResult<TemplateEntity>
                    {
                        Result = rs.Item2.ToArray(),
                        Total = rs.Item1.Single()
                    });

        public Task<Option<TemplateEntity>> GetByIdAsync(Guid templateId)
            => _dbRunner.ExecuteMultipleAsync<TemplateEntity, TemplateFieldEntity>(ProcedureNames.Templates.GetById, new { Id = templateId})
                .MapResultTo(
                    rs =>
                    {
                        var template = Optional(rs.Item1.FirstOrDefault());
                        template.IfSome(tmp => tmp.Fields = rs.Item2.ToArray());
                        return template;
                    });

        public async Task SaveAsync(TemplateEntity template, SaveTemplateOptions? options = null)
        {
            options ??= SaveTemplateOptions.Default;
            await _dbRunner.ExecuteAsync(ProcedureNames.Templates.Save, template.ToDapperParameters());
            if (options.SaveProperties)
            {
                await _dbRunner.ExecuteAsync(ProcedureNames.Templates.SaveProperties, new SaveTemplatePropertiesParameter(template));
            }
        }

        public Task DeleteAsync(Guid templateId)
            => _dbRunner.ExecuteAsync(ProcedureNames.Templates.Delete, new {Id = templateId});
    }
}
