namespace Sunday.CMS.Core.Models.Users
{
    public class ChangeAvatarModel
    {
        public string BlobIdentifier { get; set; }

        public ChangeAvatarModel(string blobIdentifier)
        {
            BlobIdentifier = blobIdentifier;
        }
    }
}
