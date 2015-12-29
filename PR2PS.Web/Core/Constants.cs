using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR2PS.Web.Core
{
    public static class Constants
    {
        static Constants()
        {
            UNIX_TIME = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
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

        public const String MIME_TEXT_PLAIN = "text/plain";
        public const String MIME_TEXT_JSON = "text/json";
        public const String MIME_TEXT_XML = "text/xml";

        public static readonly Char[] SEPARATOR_COMMA = new Char[] { ',' };

        public const String ERR_NO_USER_WITH_SUCH_NAME = "Could not find user with that name.";
        public const String ERR_NO_SUCH_SERVER = "No such server exists.";
        public const String ERR_NO_SUCH_LEVELS = "No such level collection exists.";
        public const String ERR_NO_FORM_DATA = "No form data received.";
        public const String ERR_WRONG_PASS = "The password is incorrect.";
        public const String ERR_USER_EXISTS = "User with specified username already exists.";
        public const String ERR_ALREADY_IN = "This account was already logged in. Please relog in.";
        public const String ERR_NOT_LOGGED_IN = "You need to be logged in to do that.";
        public const String ERR_NO_RIGHTS = "You lack privilegies to do this.";
        public const String ERR_BANNED = "This account or IP address has been banned by <i>{0}</i>. Reason for this ban is <i>{1}</i>.\nBan id: <i>{2}</i>\nThis ban will expire in <i>{3}</i>.";
        public const String ERR_INVALID_DURATION = "You have specified invalid duration.";
        public const String ERR_SEARCH_FAILED = "Error occured while searching through levels.";
        public const String ERR_NO_QUERY_DATA = "No query data specified.";

        public const String STR_PLAYING_ON = "Playing on ";
        public const String STR_OFFLINE = "offline";
        public const String STR_SUCCESS = "success";
        public const String STR_YEARS = " years";
        public const String STR_MONTHS = " months";
        public const String STR_DAYS = " days";
        public const String STR_HOURS = " hours";
        public const String STR_MINUTES = " minutes";
        public const String STR_SHORT_MOMENT = "a short moment";
        public const String STR_NO_REASON = "none";

        public const String SERVER_OPEN = "open";
        public const String SERVER_DOWN = "down";

        public const Double SERVER_KEEPALIVE_INTERVAL_MILLIS = 300000;
        public const Double SERVER_KEEPALIVE_TIMER_MILLIS = 60000;

        public static readonly DateTime UNIX_TIME;

        public const Char ARG_CHAR = '`';

        public const String PARTS_ALL_HATS = "1,2,3,4,5,6,7,8,9,10,11,12,13,14";
        public const String PARTS_ALL_HEADS = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39";
        public const String PARTS_ALL_BODIES = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39";
        public const String PARTS_ALL_FEET = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39";
    }
}
