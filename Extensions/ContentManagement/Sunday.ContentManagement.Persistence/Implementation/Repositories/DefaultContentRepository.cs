using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Sunday.ContentManagement.Models;
using Sunday.ContentManagement.Persistence.Application;
using Sunday.ContentManagement.Persistence.Entities;
using Sunday.ContentManagement.Persistence.Implementation.DapperParameters;
using Sunday.Core;
using Sunday.Core.Extensions;
using Sunday.DataAccess.SqlServer.Database;
using static LanguageExt.Prelude;

namespace Sunday.ContentManagement.Persistence.Implementation.Repositories
{
    [ServiceTypeOf(typeof(IContentRepository))]
    public class DefaultContentRepository : IContentRepository
    {
        private readonly StoredProcedureRunner _dbRunner;

        public DefaultContentRepository(StoredProcedureRunner dbRunner)
        {
            _dbRunner = dbRunner;
        }

        public async Task<ContentEntity[]> GetByParentAsync(Guid parentId, int parentType)
        {
            var result = await _dbRunner.ExecuteAsync<ContentEntity>
                (ProcedureNames.Contents.GetByParents, new { ParentId = parentId, ParentType = parentType });
            return result.ToArray();
        }

        public async Task<Option<ContentEntity>> GetByIdAsync(Guid contentId, GetContentOptions? options = null)
        {
            options ??= new GetContentOptions();
            var returnTypes = new List<Type> { typeof(ContentEntity) };
            if (options.IncludeVersions)
            {
                returnTypes.Add(typeof(WorkContentEntity));
                if (options.IncludeFields)
                {
                    returnTypes.Add(typeof(WorkContentEntity));
                    returnTypes.Add(typeof(ContentFieldEntity));
                }
            }
            else if (options.IncludeFields) returnTypes.Add(typeof(ContentFieldEntity));
            var result = await _dbRunner.ExecuteMultipleAsync(ProcedureNames.Contents.GetById, returnTypes,
                new { Id = contentId, options.IncludeVersions, options.IncludeFields });
            var contentOpt = Optional(result[0].FirstOrDefault() as ContentEntity);
            if (contentOpt.IsNone) return Option<ContentEntity>.None;
            var content = contentOpt.Get()!;
            if (options.IncludeVersions)
            {
                content.Versions = result[1].Select(c => c as WorkContentEntity).ToArray()!;
                if (!options.IncludeFields) return content;
                var workVersionFields = result[2].Select(f => (WorkContentFieldEntity)f).ToArray();
                content.Versions.Iter(version =>
                    version.Fields = workVersionFields.Where(f => f.WorkContentId == version.Id).ToArray());
                content.Fields = result[3].Select(f => (ContentFieldEntity)f).ToArray();
            }
            else if (options.IncludeFields)
            {
                content.Fields = result[1].Select(f => (ContentFieldEntity)f).ToArray();
            }
            return content;
        }

        public Task CreateAsync(ContentEntity content)
            => _dbRunner.ExecuteAsync(ProcedureNames.Contents.Create, new CreateContentParameter(content));

        public Task UpdateAsync(ContentEntity content)
            => _dbRunner.ExecuteAsync(ProcedureNames.Contents.Update, new UpdateContentParameter(content));

        public Task DeleteAsync(Guid contentId)
            => _dbRunner.ExecuteAsync(ProcedureNames.Contents.Delete, new { Id = contentId });

        public Task CreateNewVersionAsync(Guid contentId, Guid workVersionId, string? updatedBy = null, DateTime? updatedDate = null)
            => _dbRunner.ExecuteAsync(ProcedureNames.Contents.NewVersion, new { Id = contentId, FromVersion = workVersionId, 
                UpdatedBy = updatedBy, UpdatedDate = updatedDate ?? DateTime.Now });

        public Task PublishAsync(Guid contentId, string publishBy, DateTime? publishDate = null)
            => _dbRunner.ExecuteAsync(ProcedureNames.Contents.Publish,
                new {Id = contentId, PublishedBy = publishBy, PublishedDate = publishDate ?? DateTime.Now});
    }
}
