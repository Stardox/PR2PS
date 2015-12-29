using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR2PS.Web.Core.JSONClasses
{
    public class ErrorJSON
    {
        [JsonProperty(PropertyName = "error")]
        public String Error { get; set; }
    }
}