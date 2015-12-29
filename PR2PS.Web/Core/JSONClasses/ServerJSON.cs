using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR2PS.Web.Core.JSONClasses
{
    public class ServerJSON
    {
        [JsonProperty(PropertyName = "server_id")]
        public Int32 Server_id { get; set; }
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
        public String Guild_id { get; set; }
        [JsonProperty(PropertyName = "tournament")]
        public String Tournament { get; set; }
        [JsonProperty(PropertyName = "happy_hour")]
        public String Happy_hour { get; set; }
    }

    public class ServerListJSON
    {
        [JsonProperty(PropertyName = "servers")]
        public List<ServerJSON> Servers { get; set; }
    }
}