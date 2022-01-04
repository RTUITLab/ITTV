using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using ITTV.WPF.Core.Helpers;
using Newtonsoft.Json;

namespace ITTV.WPF.DataModel.Models
{
    //TODO: refactoring
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
        public BitmapImage Source => ImageHelper.ConvertByteToImage(byteImageList[0]);

        public string Content { get => content; }
        public string Title { get => title; }
        public List<byte[]> ByteImageList { get => byteImageList; set => byteImageList = value; }

        [JsonIgnore]
        public List<BitmapImage> ImageList {
            get
            {
                return imageList ??= ImageHelper.ConvertBytesToImages(byteImageList);
            } 
        }
    }
}