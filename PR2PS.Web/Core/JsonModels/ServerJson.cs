using Newtonsoft.Json;
using System;

namespace PR2PS.Web.Core.JsonModels
{
    public class ServerJson : IJsonModel
    {
        [JsonProperty(PropertyName = "server_id")]
        public Int32 ServerId { get; set; }

        [JsonProperty(PropertyName = "server_name")]
        public String Server_name { get; set; }

        [JsonProperty(PropertyName = "address")]
        public String Address { get; set; }

        [JsonProperty(PropertyName = "port")]
        public String Port { get; set; }

        [JsonProperty(PropertyName = "population")]
        public Int32 Population { get; set; }

        [JsonProperty(PropertyName = "status")]
        public String Status { get; set; }

        [JsonProperty(PropertyName = "guild_id")]
        public String GuildId { get; set; }

        [JsonProperty(PropertyName = "tournament")]
        public String Tournament { get; set; }

        [JsonProperty(PropertyName = "happy_hour")]
        public String HappyHour { get; set; }
    }
}