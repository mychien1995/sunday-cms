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
}
