using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Facts.Models
{
    public class Fact
    {
        [JsonProperty("used")]
        public bool Used { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("__v")]
        public long V { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        public override string ToString()
        {
            return $"{DateTime.UtcNow.ToString("o")} {Type} {Text}";
        }
    }

    public class Status
    {
        [JsonProperty("verified")]
        public bool Verified { get; set; }

        [JsonProperty("sentCount")]
        public long SentCount { get; set; }
    }
}
