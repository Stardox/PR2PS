using System;
using System.Net.Http;

namespace PR2PS.Web.Core
{
    public static class Extensions
    {
        /// <summary>
        /// Gets remote IP address from the current HTTP request.
        /// </summary>
        /// <param name="request">Current HTTP request context.</param>
        /// <returns>String IP address or null on error.</returns>
        public static String GetRemoteIPAddress(this HttpRequestMessage request)
        {
            try
            {
                return request.GetOwinContext().Request.RemoteIpAddress;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets remote port from the HTTP request.
        /// </summary>
        /// <param name="request">Current HTTP request context.</param>
        /// <returns>Port or null on error.</returns>
        public static Int32? GetRemotePort(this HttpRequestMessage request)
        {
            try
            {
                return request.GetOwinContext().Request.RemotePort;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

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
                return String.Concat(years, Constants.STR_YEARS);
            }
            else
            {
                Int32 months = (Int32)(delta.TotalDays / 30);
                if (Math.Abs(months) > 1)
                {
                    return String.Concat(months, Constants.STR_MONTHS);
                }
                else if (Math.Abs((Int32)delta.TotalDays) > 1)
                {
                    return String.Concat((Int32)delta.TotalDays, Constants.STR_DAYS);
                }
                else if (Math.Abs((Int32)delta.TotalHours) > 1)
                {
                    return String.Concat((Int32)delta.TotalHours, Constants.STR_HOURS);
                }
                else if (Math.Abs((Int32)delta.TotalMinutes) > 1)
                {
                    return String.Concat((Int32)delta.TotalMinutes, Constants.STR_MINUTES);
                }
                else
                {
                    return Constants.STR_SHORT_MOMENT;
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
            return Convert.ToInt64(dateTime.Subtract(Constants.UNIX_TIME).TotalSeconds);
        }
    }
}
