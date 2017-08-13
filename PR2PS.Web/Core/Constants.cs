using System;
using System.Collections.Generic;
using System.IO;

namespace PR2PS.Web.Core
{
    // TODO - Move to Common.
    public static class WebConstants
    {
        static WebConstants()
        {
            FILE_POLICY_XML = File.ReadAllText(PATH_POLICY_XML);
            FILE_CAMPAIGN = new Dictionary<Int32, String>();

            for (Int32 i = 1; i <= 7; i++)
            { 
                FILE_CAMPAIGN.Add(i, File.ReadAllText(Path.Combine(PATH_CAMPAIGN, i.ToString())));
            }
        }

        public static String FILE_POLICY_XML;
        public static Dictionary<Int32, String> FILE_CAMPAIGN;

        public static readonly String PATH_POLICY_XML = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "crossdomain.xml");
        public static readonly String PATH_CAMPAIGN = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "Campaign");

        public const Double SERVER_KEEPALIVE_INTERVAL_MILLIS = 300000;
        public const Double SERVER_KEEPALIVE_TIMER_MILLIS = 60000;
        public const Double SESSION_EXPIRY_MINUTES = 360;
        public const Double SESSION_CHECK_TIMER_MILLIS = 300000;
    }
}
