using Microsoft.AspNetCore.Http;
using Sunday.Core;
using Sunday.Core.Identity;
using Sunday.Core.Pipelines.Arguments;
using System;
using System.Collections.Generic;
using System.Text;

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
            var changeArg = arg as IEntityChangedArg;
            if (changeArg?.EntityChange != null)
            {
                var entity = changeArg.EntityChange;
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
}
