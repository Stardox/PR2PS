using Newtonsoft.Json;
using PR2PS.Common.Constants;
using PR2PS.DataAccess.Entities;
using System;
using System.Globalization;

namespace PR2PS.Web.Core.JsonModels
{
    public class UserInfoJson : IJsonModel
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

        // TODO - Questionable approach. Maybe this should be extension method.
        public static UserInfoJson ToUserInfoJson(Account acc)
        {
            return new UserInfoJson
            {
                UserId = acc.Id.ToString(),
                Name = acc.Username,
                Group = acc.Group.ToString(),
                LoginDate = acc.LoginDate.ToString(Other.DATE_FORMAT, CultureInfo.InvariantCulture),
                RegisterDate = acc.RegisterDate.ToString(Other.DATE_FORMAT, CultureInfo.InvariantCulture),
                Status = acc.Status,

                Rank = acc.CustomizeInfo.Rank,
                Hats = acc.CustomizeInfo.HatSeq.Split(Separators.SEPARATOR_COMMA, StringSplitOptions.RemoveEmptyEntries).Length - 1,
                Hat = acc.CustomizeInfo.Hat.ToString(),
                HatColor = acc.CustomizeInfo.HatColor.ToString(),
                HatColor2 = acc.CustomizeInfo.HatColor2.ToString(),
                Head = acc.CustomizeInfo.Head.ToString(),
                HeadColor = acc.CustomizeInfo.HeadColor.ToString(),
                HeadColor2 = acc.CustomizeInfo.HeadColor2.ToString(),
                Body = acc.CustomizeInfo.Body.ToString(),
                BodyColor = acc.CustomizeInfo.BodyColor.ToString(),
                BodyColor2 = acc.CustomizeInfo.BodyColor2.ToString(),
                Feet = acc.CustomizeInfo.Feet.ToString(),
                FeetColor = acc.CustomizeInfo.FeetColor.ToString(),
                FeetColor2 = acc.CustomizeInfo.FeetColor2.ToString(),

                GuildId = "0", // TODO - Guild id.
                GuildName = "", // TODO - Guild name.

                Friend = 0, // TODO - 1 if is friend.
                Ignored = 0 // TODO - 1 if is ignore.
            };
        }
    }
}