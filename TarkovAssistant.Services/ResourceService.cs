using TarkovAssistant.Domain;

namespace TarkovAssistant.Services
{   
    public static class ResourceService
    {
        private const string ApiUrl = "https://localhost:7296/";
        private const string ImagesUrl = "images/map/";
        private static readonly string CacheFolder = Path.Combine(Environment.CurrentDirectory, "images");

        public static async Task<string> LoadResourceAsync(string hash, ResourceKind kind)
        {
            if (string.IsNullOrWhiteSpace(hash))
            {
                return string.Empty;
            }

            var path = Path.Combine(CacheFolder, kind.ToString());

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var filename = hash + ".jpg";
            var filepath = Path.Combine(path, filename);

            if (!File.Exists(filepath))
            {
                using var client = new HttpClient();
                var bytes = await client.GetByteArrayAsync(ApiUrl + ImagesUrl + filename);
                await File.WriteAllBytesAsync(filepath, bytes);
            }

            return filepath;
        }
    }
}
