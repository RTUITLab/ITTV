using System.IO;

namespace ITTV.API.Core.Models.S3
{
    public class S3FileInfo
    {
        public S3FileInfo()
        { }

        public S3FileInfo(Stream fileData, string contentType)
        {
            FileData = fileData;
            ContentType = contentType;
        }

        public Stream FileData { get; set; }
        public string ContentType { get; set; }
    }
}