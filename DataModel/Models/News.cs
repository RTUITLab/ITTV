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
        private List<byte[]> imageList;

        public News(string uniqueId, string title, Type navigationPage, string content, List<byte[]> images) : base(uniqueId, title, navigationPage, null) 
        {
            this.content = content;
            this.ImageList = images;
        }

        [JsonIgnore]
        public BitmapImage Source
        {
            get { return ConvertToImage(ImageList)[0]; }
        }
        public string Content { get => content; }
        public List<byte[]> ImageList { get => imageList; set => imageList = value; }

        public static List<byte[]> ConvertToArray(List<ImageSource> images)
        {
            List<byte[]> bytes = new List<byte[]>();
            foreach (BitmapSource image in images)
            {
                byte[] data;
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                using (MemoryStream ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    data = ms.ToArray();
                }

                bytes.Add(data);
            }
            return bytes;
        }

        public static List<BitmapImage> ConvertToImage(List<byte[]> arrays)
        {
            List<BitmapImage> images = new List<BitmapImage>();
            foreach (byte[] array in arrays)
            {
                using (var ms = new MemoryStream(array))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                    images.Add(image);
                }
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
