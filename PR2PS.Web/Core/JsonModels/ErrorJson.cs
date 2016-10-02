using Newtonsoft.Json;
using System;

namespace PR2PS.Web.Core.JsonModels
{
    public class ErrorJson : IJsonModel
    {
        [JsonProperty(PropertyName = "error")]
        public String Error { get; set; }
    }
}