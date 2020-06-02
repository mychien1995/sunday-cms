using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core.Pipelines.Arguments
{
    public class BeforeUpdateEntityArg : PipelineArg
    {
        public object DataModel { get; set; }
        public object Entity { get; set; }
        public BeforeUpdateEntityArg(object model, object entity)
        {
            this.DataModel = model;
            this.Entity = entity;
            this.AddProperty("EntityChanged", entity);
        }
    }
}
