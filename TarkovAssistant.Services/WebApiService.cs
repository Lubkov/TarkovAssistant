using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;
using TarkovAssistant.Contracts;
using TarkovAssistant.Domain;

namespace TarkovAssistant.Services
{
    public class WebApiService : IWebApiService
    {
        private const string ImagesUrl = "images/";
        private static readonly string CacheFolder = Path.Combine(Environment.CurrentDirectory, "images");
        private readonly HttpClient _client;

        public WebApiService(HttpClient client)
        {
            this._client = client;
        }

        public async Task<List<MapDto>> GetMapsAsync()
        {
            var maps = await _client
                .GetFromJsonAsync<List<MapDto>>("Maps")
                ?? new List<MapDto>();

            await Parallel.ForEachAsync(maps, async (map, cancellationToken) =>
            {
                // update file path
                map.Picture = await LoadResourceAsync(
                    map.Picture,
                    ResourceKind.Map);
            });

            return maps;
        }

        public async Task<MapFullDto?> GetMapByIdAsync(int mapId, int? profileId)
        {
            try
            {
                var url = $"Maps/{mapId}";

                if (profileId.HasValue)
                {
                    var query = new Dictionary<string, string>
                    {
                        ["profileId"] = profileId.Value.ToString()
                    };

                    url = QueryHelpers.AddQueryString(url, query);
                }

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
            catch
            {
                throw;
            }
        }

        public async Task<MarkerFullDto?> GetMarkerByIdAsync(int id, int? profileId)
        {
            try
            {
                var url = $"Markers/{id}";

                if (profileId.HasValue)
                {
                    var query = new Dictionary<string, string>
                    {
                        ["profileId"] = profileId.Value.ToString()
                    };

                    url = QueryHelpers.AddQueryString(url, query);
                }

                var marker = await _client.GetFromJsonAsync<MarkerFullDto>(url);

                if (marker != null)
                {
                    await Parallel.ForEachAsync(marker.Resources, async (res, cancellationToken) =>
                    {
                        // update file path
                        res.Picture = await LoadResourceAsync(res.Picture, res.Kind);
                    });
                }

                return marker;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> SaveMarkerStateAsync(int profileId, int markerId, bool isSeleced, bool isFinished)
        {
            MarkerStateDto state = new MarkerStateDto
            {
                ProfileId = profileId,
                MarkerId = markerId,
                IsSeleced = isSeleced,
                IsFinished = isFinished
            };

            HttpResponseMessage response = await _client.PostAsJsonAsync("/Markers/state", state);
            
            return response.IsSuccessStatusCode;
        }

        public async Task<List<ProfileDto>> GetProfilesAsync()
        {
            return await _client
                .GetFromJsonAsync<List<ProfileDto>>("Profiles")
                ?? new List<ProfileDto>();
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
                case ResourceKind.Screenshot:
                    url = url + "marker/";
                    break;
                case ResourceKind.Quest:
                    url = url + "item/";
                    break;
                case ResourceKind.Map:
                    url = url + "map/";
                    break;
                case ResourceKind.Layer:
                    url = url + "layer/";
                    break;
                default:
                    return string.Empty;                    
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
