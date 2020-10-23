using System.Threading.Tasks;

namespace Sunday.Core.Pipelines
{
    public interface IAsyncPipelineProcessor
    {
        Task ProcessAsync(PipelineArg arg);
    }
}
