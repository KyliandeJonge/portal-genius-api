using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalGenius.Core.Models
{
    public class SearchResult<T> where T : class
    {
        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("start")]
        public int Start { get; set; }

        [JsonProperty("num")]
        public int Num { get; set; }

        [JsonProperty("nextStart")]
        public int NextStart { get; set; }

        [JsonProperty("results")]
        public T[] Results { get; set; }

    }
}
