﻿using Sunday.Core.Domain.Interfaces;

namespace Sunday.Core.Pipelines.Arguments
{
    public class BeforeUpdateEntityArg : PipelineArg
    {
        public BeforeUpdateEntityArg(IEntity entity)
        {
            Entity = entity;
        }

        public IEntity Entity { get; set; }
    }
}
