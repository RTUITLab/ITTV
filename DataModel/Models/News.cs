using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models
{
    /// <summary>
    /// Generic item data model.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "File is from Windows Store template")]
    public class News : DataPageBase
    {
        private string content;
        private List<byte[]> byteImageList;
        private List<BitmapImage> imageList;

        public News(string uniqueId, string title, Type navigationPage, string content, List<byte[]> images) : base(uniqueId, title, navigationPage, null) 
        {
            this.content = content;
            this.byteImageList = images;
        }

        [JsonIgnore]
        public BitmapImage Source
        {
            get { return ConvertByteToImage(byteImageList[0]); }
        }
        public string Content { get => content; }
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

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static News Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<News>(json);
        }
    }
}
