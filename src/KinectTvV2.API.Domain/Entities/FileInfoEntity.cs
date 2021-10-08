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
        public string Name { 
            get {
                var base64EncodedBytes = System.Convert.FromBase64String(BaseName);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
        }
    }
}