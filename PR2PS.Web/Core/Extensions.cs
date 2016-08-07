using System;
using System.Net.Http;

namespace PR2PS.Web.Core
{
    // TODO - Find out what is the best practice to extract these Owin extensions.
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
    }
}
