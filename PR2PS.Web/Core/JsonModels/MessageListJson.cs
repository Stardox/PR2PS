using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PR2PS.Web.Core.JsonModels
{
    public class MessageListJson : IJsonModel
    {
        [JsonProperty(PropertyName = "messages")]
        public List<MessageJson> Messages { get; set; }

        [JsonProperty(PropertyName = "success")]
        public Boolean Success { get; set; }
    }
}
