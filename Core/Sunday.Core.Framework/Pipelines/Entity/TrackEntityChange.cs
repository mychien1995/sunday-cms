using System;
using Microsoft.AspNetCore.Http;
using Sunday.Core.Framework.Extensions;
using Sunday.Core.Pipelines;
using Sunday.Core.Pipelines.Arguments;

namespace Sunday.Core.Framework.Pipelines.Entity
{
    public class TrackEntityChange : IPipelineProcessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TrackEntityChange(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Process(PipelineArg pipelineArg)
        {
            var currentUserName = _httpContextAccessor.CurrentUserName();
            switch (pipelineArg)
            {
                case BeforeCreateEntityArg createEntityArg:
                    {
                        var arg = createEntityArg;
                        arg.Entity.CreatedDate = DateTime.Now;
                        arg.Entity.UpdatedDate = DateTime.Now;
                        arg.Entity.Id = Guid.NewGuid();
                        arg.Entity.CreatedBy = currentUserName;
                        arg.Entity.UpdatedBy = currentUserName;
                        break;
                    }
                case BeforeUpdateEntityArg updateEntityArg:
                    {
                        var arg = updateEntityArg;
                        arg.Entity.UpdatedDate = DateTime.Now;
                        arg.Entity.UpdatedBy = currentUserName;
                        break;
                    }
            }
        }
    }
}
