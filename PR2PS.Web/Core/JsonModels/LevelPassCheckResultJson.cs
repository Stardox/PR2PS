using Newtonsoft.Json;
using System;

namespace PR2PS.Web.Core.JsonModels
{
    public class LevelPassCheckResultJson : IJsonModel
    {
        [JsonProperty(PropertyName = "access")]
        public String Access { get; set; }

        [JsonProperty(PropertyName = "level_id")]
        public String LevelId { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public String UserId { get; set; }
    }
}