using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "File is from Windows Store template")]
    public class News
    {
        private readonly List<byte[]> byteImageList;
        private List<BitmapImage> imageList;

        public News(string title,string content, List<byte[]> images)
        {
            Title = title;
            Content = content;
            byteImageList = images;
        }

        public string Content { get; }
        public string Title { get; }

        [JsonIgnore]
        public List<BitmapImage> ImageList { get
        {
            return imageList ??= ConvertBytesToImages(byteImageList);
        } }


        private BitmapImage ConvertByteToImage (byte[] array)
        {
            using var ms = new MemoryStream(array);
            
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = ms;
            image.EndInit();
            
            return image;
        }

        private List<BitmapImage> ConvertBytesToImages(IEnumerable<byte[]> arrays)
        {
            return arrays.Select(ConvertByteToImage).ToList();
        }
    }
}
