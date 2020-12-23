using System;
using Sunday.Core.Media.Application;
using Sunday.Core.Media.Domain;

namespace Sunday.Core.Media.Implementation
{
    public class RemoteBlobProvider : IBlobProvider
    {
        private readonly string _uploadFileEndpoint;
        private readonly string _apiKey;

        public RemoteBlobProvider(string uploadFileEndpoint, string apiKey)
        {
            _uploadFileEndpoint = uploadFileEndpoint;
            _apiKey = apiKey;
        }
        public void Initialize()
        {
        }

        public string Name => nameof(RemoteBlobProvider);
        public ApplicationBlob CreateBlob(string containerIdentifier, string extension)
        => new RemoteFileBlob(Guid.NewGuid().ToString("N") + extension, _uploadFileEndpoint, _apiKey);

        public ApplicationBlob GetBlob(string identifier)
        {
            throw new NotImplementedException();
        }

        public void Delete(string identifier)
        {
            throw new NotImplementedException();
        }
    }
}
