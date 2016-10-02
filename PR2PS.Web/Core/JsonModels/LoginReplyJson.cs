using Newtonsoft.Json;
using System;

namespace PR2PS.Web.Core.JsonModels
{
    public class LoginReplyJson : IJsonModel
    {
        [JsonProperty(PropertyName = "status")]
        public String Status { get; set; }

        [JsonProperty(PropertyName = "token")]
        public String Token { get; set; }

        [JsonProperty(PropertyName = "email")]
        public Boolean Email { get; set; }

        [JsonProperty(PropertyName = "ant")]
        public Boolean Ant { get; set; }

        [JsonProperty(PropertyName = "time")]
        public Int64 Time { get; set; }

        [JsonProperty(PropertyName = "lastRead")]
        public Int64 LastRead { get; set; }

        [JsonProperty(PropertyName = "lastRecv")]
        public Int64 LastRecv { get; set; }

        [JsonProperty(PropertyName = "guild")]
        public String Guild { get; set; }

        [JsonProperty(PropertyName = "guildOwner")]
        public Int32 GuildOwner { get; set; }

        [JsonProperty(PropertyName = "guildName")]
        public String GuildName { get; set; }

        [JsonProperty(PropertyName = "emblem")]
        public String Emblem { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public Int64 UserId { get; set; }
    }
}