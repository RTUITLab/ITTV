using System;

namespace ITTV.API.Core.Models
{
    public sealed class ApiFileInfo
    {
        public ApiFileInfo()
        { }

        public ApiFileInfo(string fileName, DateTime created)
        {
            FileName = fileName;
            Created = created;
        }
        public string FileName { get; set; }
        public DateTime Created { get; set; }
    }
}