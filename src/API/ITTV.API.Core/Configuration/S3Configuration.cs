namespace ITTV.API.Core.Configuration
{
    public class S3Configuration
    {
        public string Endpoint { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        
        public string BucketName { get; set; }

    }
}