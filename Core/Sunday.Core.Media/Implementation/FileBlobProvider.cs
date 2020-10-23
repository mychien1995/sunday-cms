using Microsoft.AspNetCore.Hosting;
using Sunday.Core.Media.Application;
using Sunday.Core.Media.Domain;
using System;
using System.IO;

namespace Sunday.Core.Media.Implementation
{
    public class FileBlobProvider : BlobProvider
    {
        private readonly string _path;

        public FileBlobProvider(IWebHostEnvironment webHostEnvironment, string basePath, bool useRelativePath)
        {
            this._path = useRelativePath ? Path.Combine(webHostEnvironment.WebRootPath, basePath) : basePath;
        }

        public override string Name => nameof(FileBlobProvider);

        public override ApplicationBlob CreateBlob(string containerIdentifier, string extension)
        {
            var newIdentifier = containerIdentifier.TrimEnd('\\') + "/" + Guid.NewGuid().ToString("N") + extension;
            return new FileBlob(newIdentifier, GetFilePath(newIdentifier));
        }

        public override void Delete(string identifier)
        {
            var fileInfo = new FileInfo(GetFilePath(identifier));
            if (!fileInfo.Exists)
                return;
            fileInfo.Delete();
            fileInfo.Refresh();
        }

        public override ApplicationBlob GetBlob(string identifier)
        {
            var filePath = GetFilePath(identifier);
            var fileInfo = new FileInfo(filePath);
            return (!fileInfo.Exists ? null : new FileBlob(identifier, filePath)) ?? throw new InvalidOperationException($"File path {filePath} not found");
        }

        public override void Initialize()
        {
            if (string.IsNullOrEmpty(this._path)) return;
            if (!Directory.Exists(this._path))
            {
                Directory.CreateDirectory(this._path);
            }
        }

        protected virtual string GetFilePath(string identifier)
        {
            return System.IO.Path.Combine(this._path, identifier.TrimStart('\\').TrimStart('/'));
        }
    }
}
