using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core
{
    public class PipelineArg
    {
        public PipelineArg()
        {
            Messages = new List<string>();
        }
        public bool Aborted { get; set; }
        public List<string> Messages { get; set; }
    }
}
