using Newtonsoft.Json;
using System;

namespace PR2PS.Web.Core.JsonModels
{
    public class MessageJson : IJsonModel
    {
        [JsonProperty(PropertyName = "message_id")]
        public Int64 MessageId { get; set; }

        [JsonProperty(PropertyName = "message")]
        public String Message { get; set; }

        [JsonProperty(PropertyName = "time")]
        public Int64 Time { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public Int64 UserId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public String Name { get; set; }

        [JsonProperty(PropertyName = "group")]
        public Byte Group { get; set; }
    }
}
