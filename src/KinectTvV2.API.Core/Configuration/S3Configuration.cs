namespace KinectTvV2.API.Configuration
{
    public class S3Configuration
    {
        public const string ConfigSectionName = "S3Configuration";
        
        public string Endpoint { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
    }
}