using System.Collections.Generic;

namespace ITTV.WPF.DTOs
{
    public sealed class NewsDto
    {
        public NewsDto()
        { }
        public NewsDto(string title,
            string description,
            List<byte[]> sourceImages)
        {
            Title = title;
            Description = description;
            SourceImages = sourceImages;
        }
        public string Title { get; }
        public string Description { get; }
        
        public List<byte[]> SourceImages { get; }
    }
}