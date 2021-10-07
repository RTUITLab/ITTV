namespace KinectTvV2.API.Configuration
{
    public class S3BucketOptions
    {
        public const string ConfigSectionName = "S3BucketOptions";
        
        public string BucketName { get; set; }
    }
}