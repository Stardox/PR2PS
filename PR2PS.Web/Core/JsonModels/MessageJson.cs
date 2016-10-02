using Newtonsoft.Json;
using System;

namespace PR2PS.Web.Core.JsonModels
{
    public class MessageJson : IJsonModel
    {
        [JsonProperty(PropertyName = "message_id")]
        public Int64 Message_id { get; set; }

        [JsonProperty(PropertyName = "message")]
        public String Message { get; set; }

        [JsonProperty(PropertyName = "time")]
        public Int64 Time { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public Int64 User_id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public String Name { get; set; }

        [JsonProperty(PropertyName = "group")]
        public Byte Group { get; set; }
    }
}
