namespace KinectTvV2.API.Domain.Entities
{
    public class FileEntity : Entity
    {
        private FileEntity()
        { }
        public FileEntity(string name, string path)
        {
            Name = name;
            Path = path;
        }
        public string Name { get; private set; }
        public string Path { get; private set; }
    }
}