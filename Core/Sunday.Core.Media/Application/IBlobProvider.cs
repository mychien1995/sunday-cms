using Sunday.Core.Media.Domain;

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

    public class DummyBlobProvider : IBlobProvider
    {
        public void Initialize()
        {
            
        }

        public string Name => "Dummy";
        public ApplicationBlob CreateBlob(string containerIdentifier, string extension)
        {
            return null!;
        }

        public ApplicationBlob GetBlob(string identifier)
        {
            return null!;
        }

        public void Delete(string identifier)
        {
        }
    }
}
