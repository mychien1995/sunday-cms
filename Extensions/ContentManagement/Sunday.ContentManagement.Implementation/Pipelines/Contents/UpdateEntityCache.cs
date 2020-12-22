using System.Threading.Tasks;
using Sunday.ContentManagement.Domain;
using Sunday.ContentManagement.Models;
using Sunday.Core.Application;
using Sunday.Core.Pipelines;
using Sunday.Core.Pipelines.Arguments;

namespace Sunday.ContentManagement.Implementation.Pipelines.Contents
{
    public class UpdateEntityCache : IPipelineProcessor
    {
        private readonly IRemoteEventHandler _remoteEventHandler;

        public UpdateEntityCache(IRemoteEventHandler remoteEventHandler)
        {
            _remoteEventHandler = remoteEventHandler;
        }

        public void Process(PipelineArg arg)
        {
            Task.Run(() =>
            {
                switch (arg)
                {
                    case BeforeUpdateEntityArg beforeUpdateEntityArg:
                        switch (beforeUpdateEntityArg.Entity)
                        {
                            case Rendering rendering:
                                _remoteEventHandler.Send(new RemoteEventData("rendering:updated", rendering.Id));
                                break;
                            case ApplicationLayout layout:
                                _remoteEventHandler.Send(new RemoteEventData("layout:updated", layout.Id));
                                break;
                            case ApplicationWebsite website:
                                _remoteEventHandler.Send(new RemoteEventData("website:updated", website.Id));
                                break;
                        }

                        break;
                    case BeforeDeleteEntityArg beforeDeleteEntityArg:
                        switch (beforeDeleteEntityArg.EntityType.Name)
                        {
                            case nameof(Rendering):
                                _remoteEventHandler.Send(new RemoteEventData("rendering:deleted",
                                    beforeDeleteEntityArg.EntityId));
                                break;
                            case nameof(ApplicationLayout):
                                _remoteEventHandler.Send(new RemoteEventData("layout:deleted",
                                    beforeDeleteEntityArg.EntityId));
                                break;
                            case nameof(ApplicationWebsite):
                                _remoteEventHandler.Send(new RemoteEventData("website:deleted",
                                    beforeDeleteEntityArg.EntityId));
                                break;
                        }

                        break;
                }
            });
        }
    }
}
