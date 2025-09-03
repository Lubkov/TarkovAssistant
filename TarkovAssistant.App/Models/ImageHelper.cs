using System.IO;
using System.Windows.Media.Imaging;

namespace TarkovAssistant.App.Models
{
    public static class ImageHelper
    {
        public static BitmapImage? GetPicture(byte[]? bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return null;
            }

            using var ms = new MemoryStream(bytes);
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = ms;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = ms;
            image.EndInit();
            image.Freeze();

            return image;
        }
    }
}
