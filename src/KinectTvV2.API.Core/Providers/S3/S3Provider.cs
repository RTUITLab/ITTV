using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using KinectTvV2.API.Core.Configuration;
using KinectTvV2.API.Core.Models.S3;
using Microsoft.Extensions.Options;

namespace KinectTvV2.API.Core.Providers.S3
{
    public class S3Provider : IS3Provider
    {
        private readonly IAmazonS3 _amazonS3;
        private readonly S3BucketOptions _s3BucketOptions;

        public S3Provider(IAmazonS3 amazonS3, IOptions<S3BucketOptions> s3BucketOptions)
        {
            _amazonS3 = amazonS3;
            _s3BucketOptions = s3BucketOptions.Value;
        }
        public async Task UploadFileAsync(Stream fileStream, string fileName, string directory = null)
        {
            var fileTransferUtility = new TransferUtility(_amazonS3);

            var bucketPath = BuildBucketPath(directory);

            var fileUploadRequest = new TransferUtilityUploadRequest()
            {
                CannedACL = S3CannedACL.PublicRead,
                BucketName = bucketPath,
                Key = fileName,
                InputStream = fileStream
            };
            await fileTransferUtility.UploadAsync(fileUploadRequest);
        }
        public async Task<S3FileInfo> ReadFileAsync(string fileName,
            string directory = null)
        {
            var fileTransferUtility = new TransferUtility(_amazonS3);
            
            var bucketPath = BuildBucketPath(directory);
           
            var request = new GetObjectRequest()
            {
                BucketName = bucketPath,
                Key = fileName
            };
            var objectResponse = await fileTransferUtility.S3Client.GetObjectAsync(request);
            var result = new S3FileInfo(objectResponse.ResponseStream, objectResponse.Headers.ContentType);
            return result;
        }

        private string BuildBucketPath(string directory)
        {
            var bucketPath = !string.IsNullOrWhiteSpace(directory)
                ? _s3BucketOptions.BucketName + @"/" + directory
                : _s3BucketOptions.BucketName;
            return bucketPath;
        }
    }
}