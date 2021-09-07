using System.Windows.Media.Imaging;
using KinectTvV2.Core.Helpers;

namespace KinectTvV2.Core.Services.ApiClient.Requests.GetNews
{
    public class ApiImageItem
    {
        public ApiImageItem()
        { }

        public ApiImageItem(string source,
            byte[] data, string name)
        {
            Source = source;
            Data = data;
            Name = name;
        }   
        public string Source { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }

        public BitmapImage Image => ImageHelper.ConvertByteToImage(Data);
    }
}