using Newtonsoft.Json;
using System;

namespace PR2PS.Web.Core.JsonModels
{
    public class ResultJson : IJsonModel
    {
        [JsonProperty(PropertyName = "result")]
        public String Result { get; set; }
    }
}