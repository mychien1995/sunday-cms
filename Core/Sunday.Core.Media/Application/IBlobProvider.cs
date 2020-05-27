using Sunday.Core.Media.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core.Media.Application
{
    public interface IBlobProvider
    {
        void Initialize();

        string Name { get; }

        ApplicationBlob CreateBlob(string containerIdentifier, string extension);

        ApplicationBlob GetBlob(string identifier);

        void Delete(string identifier);
    }
}
