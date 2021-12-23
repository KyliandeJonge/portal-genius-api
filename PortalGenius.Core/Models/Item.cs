using System.Text.Json.Serialization;

namespace PortalGenius.Core.Models
{
    public class Item
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("owner")]
        public string Owner { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("created")]
        public string Created { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
