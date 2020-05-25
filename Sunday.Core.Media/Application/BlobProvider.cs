using Sunday.Core.Media.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core.Media.Application
{
    public abstract class BlobProvider : IBlobProvider
    {
        public abstract string Name { get; }

        public abstract ApplicationBlob CreateBlob(string containerIdentifier, string extension);

        public abstract void Delete(string identifier);

        public abstract ApplicationBlob GetBlob(string identifier);

        public abstract void Initialize();
    }
}
