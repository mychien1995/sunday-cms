using System.IO;

namespace Sunday.Core.Media.Domain
{
    public class FileBlob : ApplicationBlob
    {
        public string FilePath { get; set; }
        public FileBlob()
        {

        }
        public FileBlob(string identifier, string filePath)
         : base(identifier)
        {
            this.FilePath = filePath;
        }

        public override Stream OpenRead()
        {
            return new FileStream(this.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public override Stream OpenWrite()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(this.FilePath));
            if (!directoryInfo.Exists)
                directoryInfo.Create();
            return new FileStream(this.FilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
        }

        public override void Write(Stream data)
        {
            using (Stream destination = this.OpenWrite())
                data.CopyTo(destination);
        }
    }
}
