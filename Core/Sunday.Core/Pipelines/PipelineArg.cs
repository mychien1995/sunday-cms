using System.Collections.Generic;

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
        public Dictionary<string, object> PropertyBag { get; private set; }

        public PipelineArg AddProperty(string key, object value)
        {
            if (PropertyBag == null)
                PropertyBag = new Dictionary<string, object>();
            if (PropertyBag.ContainsKey(key))
                PropertyBag.Remove(key);
            PropertyBag.Add(key, value);
            return this;
        }
        public void AddMessage(string msg)
        {
            if (Messages == null)
                Messages = new List<string>();
            Messages.Add(msg);
        }

        public object this[string key]
        {
            get
            {
                if (PropertyBag == null)
                    PropertyBag = new Dictionary<string, object>();
                if (PropertyBag.ContainsKey(key))
                    return PropertyBag[key];
                return null;
            }
            set
            {
                AddProperty(key, value);
            }
        }
    }
}
