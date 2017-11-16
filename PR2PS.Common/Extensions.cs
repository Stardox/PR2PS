using PR2PS.Common.Constants;
using PR2PS.Common.Cryptography;
using PR2PS.Common.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;

namespace PR2PS.Common.Extensions
{
    public static class Base64Extensions
    {
        public static String ToBase64(this String data)
        {
            return Encoding.UTF8.GetBytes(data).ToBase64();
        }

        public static String ToBase64(this Byte[] data)
        {
            return Convert.ToBase64String(data);
        }

        public static String FromBase64ToString(this String data)
        {
            return Encoding.UTF8.GetString(data.FromBase64ToArray());
        }

        public static Byte[] FromBase64ToArray(this String data)
        {
            return Convert.FromBase64String(data);
        }
    }

    public static class UrlExtensions
    {
        public static String ToUrlEncodedString(this String data)
        {
            return WebUtility.UrlEncode(data);
        }
    }

    // What about more appropriate name?
    public static class Other
    {
        /// <summary>
        /// Returns human readable ban expiration information.
        /// </summary>
        /// <param name="banExpiration">DateTime when ban expires.</param>
        /// <returns>Human readable ban expiration information. Example: 3 hours</returns>
        public static String GetPrettyBanExpirationString(this DateTime banExpiration)
        {
            TimeSpan delta = banExpiration - DateTime.UtcNow;

            Int32 years = (Int32)(delta.TotalDays / 365);
            if (Math.Abs(years) > 1)
            {
                return String.Concat(years, StatusMessages.STR_YEARS);
            }
            else
            {
                Int32 months = (Int32)(delta.TotalDays / 30);
                if (Math.Abs(months) > 1)
                {
                    return String.Concat(months, StatusMessages.STR_MONTHS);
                }
                else if (Math.Abs((Int32)delta.TotalDays) > 1)
                {
                    return String.Concat((Int32)delta.TotalDays, StatusMessages.STR_DAYS);
                }
                else if (Math.Abs((Int32)delta.TotalHours) > 1)
                {
                    return String.Concat((Int32)delta.TotalHours, StatusMessages.STR_HOURS);
                }
                else if (Math.Abs((Int32)delta.TotalMinutes) > 1)
                {
                    return String.Concat((Int32)delta.TotalMinutes, StatusMessages.STR_MINUTES);
                }
                else
                {
                    return StatusMessages.STR_SHORT_MOMENT;
                }
            }
        }

        /// <summary>
        /// Gets number of seconds that have elapsed since Unix time until specified DateTime.
        /// </summary>
        /// <param name="dateTime">DateTime lol.</param>
        /// <returns>Number of seconds since 1.1.1970 until specified DateTime lol.</returns>
        public static Int64 GetSecondsSinceUnixTime(this DateTime dateTime)
        {
            return Convert.ToInt64(dateTime.Subtract(Constants.Other.UNIX_TIME).TotalSeconds);
        }

        /// <summary>
        /// Returns the value of Description annotation attribute applied to enumeration value.
        /// </summary>
        /// <param name="enumVal">Enumeration value (ideally annotated with Description attribute).</param>
        /// <returns>Value of Description attribute or null.</returns>
        public static String GetEnumDescription(this Enum enumVal)
        {
            return (enumVal?.GetType().GetField(enumVal.ToString())?.GetCustomAttributes(typeof(DescriptionAttribute), true)?.FirstOrDefault() as DescriptionAttribute)?.Description;
        }

        /// <summary>
        /// Returns value of enumeration type annotated with Description attribute whose description matches the given string. 
        /// </summary>
        /// <typeparam name="T">Enumeration type.</typeparam>
        /// <param name="description">Description string to look for.</param>
        /// <param name="defaultValue">Default value which will be returned if type parameter is not a enum or if no description match was found.</param>
        /// <returns></returns>
        public static T FromEnumDescription<T>(this String description, T defaultValue) where T : struct
        {
            Type type = typeof(T);

            if (!type.IsEnum)
            {
                return defaultValue;
            }

            T? found = (T?)type.GetFields().FirstOrDefault(f => String.CompareOrdinal((f.GetCustomAttributes(typeof(DescriptionAttribute), true)?.FirstOrDefault() as DescriptionAttribute)?.Description, description) == 0)?.GetValue(null);
            if (found.HasValue)
            {
                return found.Value;
            }

            return defaultValue;
        }

        /// <summary>
        /// Processes list of levels and returns its string representation ready to be sent to client.
        /// </summary>
        /// <param name="levels">List of LevelRowDTO.</param>
        /// <returns></returns>
        public static String GetLevelListString(this IList<LevelRowDTO> levels)
        {
            if (!levels.Any())
            {
                return String.Empty;
            }

            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < levels.Count; i++)
            {
                LevelRowDTO current = levels[i];

                // TODO - Rewrite this shit. Use dictionary or something.
                // Yes, I know that string interpolation exists, but I dont like it.
                sb.AppendFormat(
                    "levelID{0}={1}&version{0}={2}&title{0}={3}&rating{0}={4}&playCount{0}={5}&minLevel{0}={6}&note{0}={7}&userName{0}={8}&group{0}={9}&live{0}={10}&pass{0}={11}&type{0}={12}{13}",
                    i,
                    current.LevelId,
                    current.Version,
                    current.Title.ToUrlEncodedString(),
                    current.Rating.ToString(StringFormat.DECIMAL_TWO, CultureInfo.InvariantCulture) ?? String.Empty,
                    current.PlayCount,
                    current.MinRank,
                    current.Note.ToUrlEncodedString(),
                    current.Username.ToUrlEncodedString(),
                    current.Group,
                    Convert.ToInt32(current.IsPublished),
                    current.HasPass ? StatusMessages.ONE : String.Empty,
                    current.GameMode.GetEnumDescription().FirstOrDefault() + String.Empty,
                    (i < levels.Count - 1) ? Separators.AMPERSAND : String.Empty);
            }

            using (MD5Wrapper md5 = new MD5Wrapper())
            {
                String hash = md5.GetHashedString(String.Concat(sb.ToString(), Pepper.LEVEL_LIST));
                sb.Append(String.Concat("&hash=", hash));
            }

            return sb.ToString();
        }
    }
}
