using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace KinectTvV2.Core.Helpers
{
    public static class ImageHelper
    {
        public static BitmapImage ConvertByteToImage(byte[] array)
        {
            using var ms = new MemoryStream(array);

            var image = new BitmapImage
            {
                CacheOption = BitmapCacheOption.OnLoad,
                StreamSource = ms
            };
            
            return image;
        }

        public static List<BitmapImage> ConvertBytesToImages(IEnumerable<byte[]> arrays)
        {
            return arrays.Select(ConvertByteToImage).ToList();
        }
    }
}