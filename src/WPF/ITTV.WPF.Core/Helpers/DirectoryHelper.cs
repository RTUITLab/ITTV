﻿using System;
using System.IO;
using System.Linq;

namespace ITTV.WPF.Core.Helpers
{
    public static class DirectoryHelper
    {
        public static Uri GetExecuteFilePath(string directoryPath)
        {
            const string executeFileExtension = ".exe";

            var files = Directory.GetFiles(directoryPath);

            var executeFile = files.SingleOrDefault(x => Path.GetExtension(x) == executeFileExtension);

            if (executeFile == null)
                throw new ArgumentException("Execution file was not found or there were several of them.");
            //TODO: Add handle exception 

            return new Uri(Path.Combine(directoryPath, executeFile));
        }
    }
}