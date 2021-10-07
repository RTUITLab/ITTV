namespace KinectTvV2.API.Domain.Entities
{
    public class FileInfoEntity : Entity
    {
        private FileInfoEntity()
        { }
        public FileInfoEntity(string name, string path)
        {
            Name = name;
            Path = path;
        }
        public string Name { get; private set; }
        public string Path { get; private set; }
    }
}