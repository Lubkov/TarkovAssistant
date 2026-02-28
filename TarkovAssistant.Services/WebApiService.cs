using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;
using TarkovAssistant.Contracts;
using TarkovAssistant.Domain;

namespace TarkovAssistant.Services
{
    public class WebApiService : IWebApiService
    {
        //private const string ApiUrl = "https://localhost:7296/";
        private const string ImagesUrl = "images/";
        private static readonly string CacheFolder = Path.Combine(Environment.CurrentDirectory, "images");
        private readonly HttpClient _client;

        public WebApiService(HttpClient client)
        {
            this._client = client;
        }

        public async Task<List<MapSummaryDto>> GetMapsFromWebApiAsync()
        {
            var maps = await _client
                .GetFromJsonAsync<List<MapSummaryDto>>("Maps")
                ?? new List<MapSummaryDto>();

            await Parallel.ForEachAsync(maps, async (map, cancellationToken) =>
            {
                // update file path
                map.Picture = await LoadResourceAsync(
                    map.Picture,
                    ResourceKind.Map);
            });

            return maps;
        }

        public async Task<MapFullDto?> GetMapByIdFromWebApiAsync(int mapId, int? profileId)
        {
            //var query = new Dictionary<string, string>
            //{
            //    { "id", mapId.ToString() }
            //};

            var url = QueryHelpers.AddQueryString("Maps", "id", mapId.ToString());
            var map = await _client.GetFromJsonAsync<MapFullDto>(url);

            if (map != null)
            {
                await Parallel.ForEachAsync(map.Layers, async (layer, cancellationToken) =>
                {
                    // update file path
                    layer.Picture = await LoadResourceAsync(
                        layer.Picture,
                        ResourceKind.Layer);
                });
            }

            return map;
        }

        private async Task<string> LoadResourceAsync(string hash, ResourceKind kind)
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
            var url = ImagesUrl;
            switch (kind)
            {
                case ResourceKind.Map:
                    url = url + "map/";
                    break;
                case ResourceKind.Layer:
                    url = url + "layer/";
                    break;
            }

            if (!File.Exists(filepath))
            {             
                var bytes = await _client.GetByteArrayAsync(url + filename);
                await File.WriteAllBytesAsync(filepath, bytes);
            }

            return filepath;
        }
    }
}
