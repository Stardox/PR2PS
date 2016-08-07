using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR2PS.Common
{
    public static class Extensions
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
    }
}
