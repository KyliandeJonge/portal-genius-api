using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PortalGenius.WPF.Models
{
    public class Item
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("created")]
        public object Created { get; set; }

        [JsonProperty("modified")]
        public object Modified { get; set; }

        [JsonProperty("guid")]
        public object Guid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("typeKeywords")]
        public List<string> TypeKeywords { get; set; }

        [JsonProperty("description")]
        public object Description { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("snippet")]
        public string Snippet { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("documentation")]
        public object Documentation { get; set; }

        [JsonProperty("extent")]
        public List<List<double>> Extent { get; set; }

        [JsonProperty("categories")]
        public List<string> Categories { get; set; }

        [JsonProperty("spatialReference")]
        public string SpatialReference { get; set; }

        [JsonProperty("accessInformation")]
        public object AccessInformation { get; set; }

        [JsonProperty("licenseInfo")]
        public object LicenseInfo { get; set; }

        [JsonProperty("culture")]
        public string Culture { get; set; }

        [JsonProperty("properties")]
        public object Properties { get; set; }

        [JsonProperty("advancedSettings")]
        public object AdvancedSettings { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("proxyFilter")]
        public object ProxyFilter { get; set; }

        [JsonProperty("access")]
        public string Access { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("subInfo")]
        public int SubInfo { get; set; }

        [JsonProperty("appCategories")]
        public List<object> AppCategories { get; set; }

        [JsonProperty("industries")]
        public List<object> Industries { get; set; }

        [JsonProperty("languages")]
        public List<object> Languages { get; set; }

        [JsonProperty("largeThumbnail")]
        public object LargeThumbnail { get; set; }

        [JsonProperty("banner")]
        public object Banner { get; set; }

        [JsonProperty("screenshots")]
        public List<object> Screenshots { get; set; }

        [JsonProperty("listed")]
        public bool Listed { get; set; }

        [JsonProperty("numComments")]
        public int NumComments { get; set; }

        [JsonProperty("numRatings")]
        public int NumRatings { get; set; }

        [JsonProperty("avgRating")]
        public int AvgRating { get; set; }

        [JsonProperty("numViews")]
        public int NumViews { get; set; }

        [JsonProperty("scoreCompleteness")]
        public int ScoreCompleteness { get; set; }

        [JsonProperty("groupDesignations")]
        public object GroupDesignations { get; set; }

        [JsonProperty("contentOrigin")]
        public string ContentOrigin { get; set; }
    }
}
