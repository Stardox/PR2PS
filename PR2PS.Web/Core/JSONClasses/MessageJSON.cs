using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR2PS.Web.Core.JSONClasses
{
    public class MessageJSON
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

    public class MessageListJSON
    {
        [JsonProperty(PropertyName = "messages")]
        public List<MessageJSON> Messages { get; set; }

        [JsonProperty(PropertyName = "success")]
        public Boolean Success { get; set; }
    }
}
