using System;
using System.ComponentModel;
using System.Linq;
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
                return String.Concat(years, Constants.StatusMessages.STR_YEARS);
            }
            else
            {
                Int32 months = (Int32)(delta.TotalDays / 30);
                if (Math.Abs(months) > 1)
                {
                    return String.Concat(months, Constants.StatusMessages.STR_MONTHS);
                }
                else if (Math.Abs((Int32)delta.TotalDays) > 1)
                {
                    return String.Concat((Int32)delta.TotalDays, Constants.StatusMessages.STR_DAYS);
                }
                else if (Math.Abs((Int32)delta.TotalHours) > 1)
                {
                    return String.Concat((Int32)delta.TotalHours, Constants.StatusMessages.STR_HOURS);
                }
                else if (Math.Abs((Int32)delta.TotalMinutes) > 1)
                {
                    return String.Concat((Int32)delta.TotalMinutes, Constants.StatusMessages.STR_MINUTES);
                }
                else
                {
                    return Constants.StatusMessages.STR_SHORT_MOMENT;
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
    }
}
