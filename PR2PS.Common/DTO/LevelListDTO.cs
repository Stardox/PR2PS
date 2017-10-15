using PR2PS.Common.Constants;
using PR2PS.Common.Cryptography;
using PR2PS.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PR2PS.Common.DTO
{
    public class LevelListDTO : List<LevelRowDTO>
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.Count; i++)
            {
                LevelRowDTO current = this[i];

                // TODO - Rewrite this shit. Use dictionary or something.
                // Yes, I know that string interpolation exists, but I dont like it.
                sb.AppendFormat(
                    "levelID{0}={1}&version{0}={2}&title{0}={3}&rating{0}={4}&playCount{0}={5}&minLevel{0}={6}&note{0}={7}&userName{0}={8}&group{0}={9}&live{0}={10}&pass{0}={11}&type{0}={12}{13}",
                    i,
                    current.LevelId,
                    current.Version,
                    current.Title,
                    current.Rating.ToString(StringFormat.DECIMAL_TWO, CultureInfo.InvariantCulture) ?? String.Empty,
                    current.PlayCount,
                    current.MinRank,
                    current.Note,
                    current.Username,
                    current.Group,
                    Convert.ToInt32(current.IsPublished),
                    current.HasPass ? StatusMessages.ONE : String.Empty,
                    current.GameMode.GetEnumDescription().FirstOrDefault() + String.Empty,
                    (i < this.Count - 1) ? Separators.AMPERSAND : String.Empty);
            }

            using (MD5Wrapper md5 = new MD5Wrapper())
            {
                String hash = md5.GetHashedString(String.Concat(sb.ToString(), Pepper.LIST_OF_LEVELS));
                sb.Append(String.Concat("&hash=", hash));
            }

            return sb.ToString();
        }
    }
}
