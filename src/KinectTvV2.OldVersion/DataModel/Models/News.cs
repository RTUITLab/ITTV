using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
namespace Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models
{
    public class News
    {
        private string title;
        private string content;
        private List<byte[]> byteImageList;
        private List<BitmapImage> imageList;

        public News(string title,string content, List<byte[]> images)
        {
            this.title = title;
            this.content = content;
            byteImageList = images;
        }

        [JsonIgnore]
        public BitmapImage Source => ConvertByteToImage(byteImageList[0]);

        public string Content { get => content; }
        public string Title { get => title; }
        public List<byte[]> ByteImageList { get => byteImageList; set => byteImageList = value; }

        [JsonIgnore]
        public List<BitmapImage> ImageList { get { if (imageList == null) { imageList = ConvertBytesToImages(byteImageList); } return imageList; } }


        private BitmapImage ConvertByteToImage (byte[] array)
        {
            using (var ms = new MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();

                return image;
            }
        }

        private List<BitmapImage> ConvertBytesToImages(List<byte[]> arrays)
        {
            List<BitmapImage> images = new List<BitmapImage>();
            foreach (byte[] array in arrays)
            {
                images.Add(ConvertByteToImage(array));
            }
            return images;
        }
    }
}