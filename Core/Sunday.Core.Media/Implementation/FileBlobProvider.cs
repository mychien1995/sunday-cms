using Sunday.Core.Media.Application;
using Sunday.Core.Media.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;

namespace Sunday.Core.Media.Implementation
{
    public class FileBlobProvider : BlobProvider
    {
        private string _path;
        private bool _useRelativePath;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileBlobProvider(IWebHostEnvironment webHostEnvironment, string basePath, string useRelativePath)
        {
            this._webHostEnvironment = webHostEnvironment;
            this._useRelativePath = bool.Parse(useRelativePath);
            if (this._useRelativePath)
            {
                this._path = Path.Combine(this._webHostEnvironment.WebRootPath, basePath);
            }
            else
            {
                this._path = basePath;
            }
        }

        public override string Name => nameof(FileBlobProvider);

        public override ApplicationBlob CreateBlob(string containerIdentifier, string extension)
        {
            var newIdentifider = containerIdentifier.TrimEnd('\\') + "/" + Guid.NewGuid().ToString("N") + extension;
            return new FileBlob(newIdentifider, GetFilePath(newIdentifider));
        }

        public override void Delete(string identifier)
        {
            FileInfo fileInfo = new FileInfo(GetFilePath(identifier));
            if (!fileInfo.Exists)
                return;
            fileInfo.Delete();
            fileInfo.Refresh();
        }

        public override ApplicationBlob GetBlob(string identifier)
        {
            var filePath = GetFilePath(identifier);
            FileInfo fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
                return null;
            return new FileBlob(identifier, filePath);
        }

        public override void Initialize()
        {
            if (!string.IsNullOrEmpty(this._path))
            {
                if (!Directory.Exists(this._path))
                {
                    Directory.CreateDirectory(this._path);
                }
            }
        }

        protected virtual string GetFilePath(string identifier)
        {
            return System.IO.Path.Combine(this._path, identifier.TrimStart('\\').TrimStart('/'));
        }
    }
}
