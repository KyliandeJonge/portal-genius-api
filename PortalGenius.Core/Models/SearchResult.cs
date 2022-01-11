
using System.Text.Json.Serialization;

namespace PortalGenius.Core.Models
{
    public class SearchResult<T> where T : class
    {
        [JsonPropertyName("query")]
        public string Query { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("start")]
        public int Start { get; set; }

        [JsonPropertyName("num")]
        public int Num { get; set; }

        [JsonPropertyName("nextStart")]
        public int NextStart { get; set; }

        [JsonPropertyName("results")]
        public T[] Results { get; set; }

    }
}
