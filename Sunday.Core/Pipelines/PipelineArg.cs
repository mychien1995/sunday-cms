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
            PropertyBag = new Dictionary<string, object>();
        }
        public bool Aborted { get; set; }
        public List<string> Messages { get; set; }
        public Dictionary<string,object> PropertyBag { get; private set; }

        public void AddProperty(string key, object value)
        {
            if (PropertyBag == null)
                PropertyBag = new Dictionary<string, object>();
            if (PropertyBag.ContainsKey(key))
                PropertyBag.Remove(key);
            PropertyBag.Add(key, value);
        }
        public void AddMessage(string msg)
        {
            if (Messages == null)
                Messages = new List<string>();
            Messages.Add(msg);
        }
    }
}
