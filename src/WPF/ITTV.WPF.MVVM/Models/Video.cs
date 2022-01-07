using System;

namespace ITTV.WPF.MVVM.Models
{
    public class Video
    {
        public Video()
        { }
        public Video(Uri path)
        {
            Name = System.IO.Path.GetFileName(path.AbsoluteUri);
            Path = path;
        }
        public string Name { get; set; }
        public Uri Path { get; set; }
    }
}