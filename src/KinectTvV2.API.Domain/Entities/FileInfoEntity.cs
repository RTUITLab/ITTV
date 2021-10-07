using KinectTvV2.API.Core.Helpers;

namespace KinectTvV2.API.Domain.Entities
{
    public class FileInfoEntity : Entity
    {
        private FileInfoEntity()
        { }
        public FileInfoEntity(string baseName)
        {
            BaseName = baseName;
        }
        public string BaseName { get; private set; }
        public string Name => Base64Helper.Decode(BaseName);
    }
}