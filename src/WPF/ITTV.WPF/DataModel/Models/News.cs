using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;

namespace ITTV.WPF.DataModel.Models
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
        public List<BitmapImage> ImageList {
            get
            {
                return imageList ??= ConvertBytesToImages(byteImageList);
            } 
        }

        private static BitmapImage ConvertByteToImage (byte[] array)
        {
            using var ms = new MemoryStream(array);
            var image = new BitmapImage();
            
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }

        private static List<BitmapImage> ConvertBytesToImages(IEnumerable<byte[]> arrays) 
            => arrays.Select(ConvertByteToImage)
                .ToList();
    }
}