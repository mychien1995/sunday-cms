using Microsoft.AspNetCore.Http;
using Sunday.Core.Domain;
using Sunday.Core.Domain.Identity;
using Sunday.Core.Pipelines.Arguments;
using System;
using System.Threading.Tasks;

namespace Sunday.Core.Pipelines.EntityChanged
{
    public class TrackEntityChange
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TrackEntityChange(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public void Process(PipelineArg arg)
        {
            DoProcess(arg);
        }

        public async Task ProcessAsync(PipelineArg arg)
        {
            DoProcess(arg);
        }

        private void DoProcess(PipelineArg arg)
        {
            var changeArg = arg as IEntityChangedArg;
            var entity = changeArg?.EntityChange ?? arg["EntityChanged"] as IEntity;
            if (entity == null) return;
            var now = DateTime.Now;
            if (entity.ID == 0)
            {
                entity.CreatedDate = now;
            }
            entity.UpdatedDate = now;
            var currentUser = _httpContextAccessor.HttpContext.User as IApplicationUserPrincipal;
            if (currentUser == null) return;
            entity.UpdatedBy = currentUser.Username;
            if (entity.ID == 0)
                entity.CreatedBy = currentUser.Username;
        }
    }
}
