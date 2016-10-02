using Newtonsoft.Json;
using System;

namespace PR2PS.Web.Core.JsonModels
{
    public class LoginDataJson : IJsonModel
    {
        [JsonProperty(PropertyName = "remember")]
        public Boolean Remember { get; set; }

        [JsonProperty(PropertyName = "user_name")]
        public String UserName { get; set; }

        [JsonProperty(PropertyName = "domain")]
        public String Domain { get; set; }

        [JsonProperty(PropertyName = "login_id")]
        public Int32 LoginId { get; set; }

        [JsonProperty(PropertyName = "version")]
        public String Version { get; set; }

        [JsonProperty(PropertyName = "user_pass")]
        public String UserPass { get; set; }

        [JsonProperty(PropertyName = "server")]
        public ServerJson Server { get; set; }

        [JsonProperty(PropertyName = "login_code")]
        public String LoginCode { get; set; }
    }
}