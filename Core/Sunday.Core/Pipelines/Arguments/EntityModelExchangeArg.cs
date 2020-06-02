using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core.Pipelines.Arguments
{
    public class EntityModelExchangeArg : PipelineArg
    {
        public object Model { get; set; }
        public object Entity { get; set; }
        public EntityModelExchangeArg(object model, object entity)
        {
            this.Model = model;
            this.Entity = entity;
            this.AddProperty("Model", model);
            this.AddProperty("Entity", entity);
        }
    }
}
