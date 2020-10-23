using System.Collections.Generic;

namespace Sunday.Core.Pipelines
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
        public Dictionary<string, object> PropertyBag { get; private set; }

        public PipelineArg AddProperty(string key, object value)
        {
            PropertyBag ??= new Dictionary<string, object>();
            if (PropertyBag.ContainsKey(key))
                PropertyBag.Remove(key);
            PropertyBag.Add(key, value);
            return this;
        }
        public void AddMessage(string msg)
        {
            Messages ??= new List<string>();
            Messages.Add(msg);
        }

        public object? this[string key]
        {
            get
            {
                PropertyBag ??= new Dictionary<string, object>();
                return PropertyBag.ContainsKey(key) ? PropertyBag[key] : null;
            }
            set => AddProperty(key, value!);
        }
    }
}
