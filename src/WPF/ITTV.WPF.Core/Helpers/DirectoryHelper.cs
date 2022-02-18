using System;
using System.IO;
using System.Linq;

namespace ITTV.WPF.Core.Helpers
{
    public static class DirectoryHelper
    {
        public static Uri GetExecuteFilePath(string directoryPath)
        {
            const string executeFileExtension = "*.exe";

            var files = Directory.GetFiles(directoryPath, executeFileExtension);

            var executeFile = files.SingleOrDefault();

            if (executeFile == null)
                throw new ArgumentException("Execution file was not found or there were several of them.");
            
            return new Uri(Path.Combine(directoryPath, executeFile));
        }
    }
}