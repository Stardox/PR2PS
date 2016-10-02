using Newtonsoft.Json;
using System.Collections.Generic;

namespace PR2PS.Web.Core.JsonModels
{
    public class ServerListJson : IJsonModel
    {
        [JsonProperty(PropertyName = "servers")]
        public List<ServerJson> Servers { get; set; }
    }
}
