using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PortalGenius.Core.Models
{
    public class Item
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("created")]
        //[JsonConverter(typeof(UnixDateTimeConverter))]
        public string Created { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
