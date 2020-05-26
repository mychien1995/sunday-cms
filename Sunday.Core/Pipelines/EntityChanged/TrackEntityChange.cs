using Microsoft.AspNetCore.Http;
using Sunday.Core;
using Sunday.Core.Domain;
using Sunday.Core.Identity;
using Sunday.Core.Pipelines.Arguments;
using System;
using System.Collections.Generic;
using System.Text;
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
            IEntity entity;
            var changeArg = arg as IEntityChangedArg;
            entity = changeArg?.EntityChange;
            if (entity == null)
            {
                entity = arg["EntityChanged"] as IEntity;
            }
            if (entity == null) return;
            var now = DateTime.Now;
            if (entity.ID == 0)
            {
                entity.CreatedDate = now;
            }
            entity.UpdatedDate = now;
            var currentUser = _httpContextAccessor.HttpContext.User as ApplicationUserPrincipal;
            if (currentUser != null)
            {
                entity.UpdatedBy = currentUser.Username;
                if (entity.ID == 0)
                    entity.CreatedBy = currentUser.Username;
            }
        }
    }
}
