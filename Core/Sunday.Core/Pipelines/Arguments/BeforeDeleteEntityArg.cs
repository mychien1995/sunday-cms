namespace Sunday.Core.Pipelines.Arguments
{
    public class BeforeDeleteEntityArg : PipelineArg
    {
        public int EntityId { get; set; }
        public BeforeDeleteEntityArg(int id)
        {
            this.EntityId = id;
            this.AddProperty("EntityId", id);
        }
    }
}
