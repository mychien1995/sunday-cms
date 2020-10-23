using System;
using System.IO;

namespace Sunday.Core.Media.Domain
{
    public class ApplicationBlob
    {
        public string Identifier { get; set; }
        public ApplicationBlob(string identifier)
        {
            this.Identifier = identifier;
        }
        public virtual Stream OpenRead()
        {
            throw new NotImplementedException();
        }

        public virtual Stream OpenWrite()
        {
            throw new NotImplementedException();
        }

        public virtual void Write(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
