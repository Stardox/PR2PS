using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR2PS.Web.Core.JSONClasses
{
    public class UserInfoJSON
    {
        [JsonProperty(PropertyName = "rank")]
        public Int32 Rank { get; set; }
        [JsonProperty(PropertyName = "hats")]
        public Int32 Hats { get; set; }
        [JsonProperty(PropertyName = "group")]
        public String Group { get; set; }
        [JsonProperty(PropertyName = "friend")]
        public Int32 Friend { get; set; }
        [JsonProperty(PropertyName = "ignored")]
        public Int32 Ignored { get; set; }
        [JsonProperty(PropertyName = "status")]
        public String Status { get; set; }
        [JsonProperty(PropertyName = "loginDate")]
        public String LoginDate { get; set; }
        [JsonProperty(PropertyName = "registerDate")]
        public String RegisterDate { get; set; }
        [JsonProperty(PropertyName = "hat")]
        public String Hat { get; set; }
        [JsonProperty(PropertyName = "head")]
        public String Head { get; set; }
        [JsonProperty(PropertyName = "body")]
        public String Body { get; set; }
        [JsonProperty(PropertyName = "feet")]
        public String Feet { get; set; }
        [JsonProperty(PropertyName = "hatColor")]
        public String HatColor { get; set; }
        [JsonProperty(PropertyName = "headColor")]
        public String HeadColor { get; set; }
        [JsonProperty(PropertyName = "bodyColor")]
        public String BodyColor { get; set; }
        [JsonProperty(PropertyName = "feetColor")]
        public String FeetColor { get; set; }
        [JsonProperty(PropertyName = "guildId")]
        public String GuildId { get; set; }
        [JsonProperty(PropertyName = "guildName")]
        public String GuildName { get; set; }
        [JsonProperty(PropertyName = "name")]
        public String Name { get; set; }
        [JsonProperty(PropertyName = "userId")]
        public String UserId { get; set; }
        [JsonProperty(PropertyName = "hatColor2")]
        public String HatColor2 { get; set; }
        [JsonProperty(PropertyName = "headColor2")]
        public String HeadColor2 { get; set; }
        [JsonProperty(PropertyName = "bodyColor2")]
        public String BodyColor2 { get; set; }
        [JsonProperty(PropertyName = "feetColor2")]
        public String FeetColor2 { get; set; }
    }
}