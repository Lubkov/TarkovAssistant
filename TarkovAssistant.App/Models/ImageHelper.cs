using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.IdentityModel.Tokens;

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

            using var stream = new MemoryStream(bytes);
            return GetPicture(stream);
        }

        public static BitmapImage? GetPicture(string filename)
        {
            if (filename.IsNullOrEmpty())
            {
                return null;
            }

            FileInfo file = new FileInfo(filename);
            if (!file.Exists)
            {
                return null;
            }

            using var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
            return GetPicture(stream);
        }

        public static BitmapImage? GetPicture(Stream stream)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = stream;
            image.EndInit();
            image.Freeze();

            return image;
        }
    }
}
