using System;

// TODO - Consider using getters or static readonly fields. Const keyword can be dangerous.
namespace PR2PS.Common.Constants
{
    public static class BodyParts
    {
        public const String PARTS_ALL_HATS = "1,2,3,4,5,6,7,8,9,10,11,12,13,14";
        public const String PARTS_ALL_HEADS = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39";
        public const String PARTS_ALL_BODIES = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39";
        public const String PARTS_ALL_FEET = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39";
    }

    public static class ErrorMessages
    {
        public const String ERR_NO_USER_WITH_SUCH_NAME = "Could not find user with that name.";
        public const String ERR_NO_SERVERS = "No servers available.";
        public const String ERR_NO_SUCH_SERVER = "No such server exists.";
        public const String ERR_NO_SUCH_LEVELS = "No such level collection exists.";
        public const String ERR_NO_FORM_DATA = "No form data received.";
        public const String ERR_NO_LOGIN_DATA = "No login data received.";
        public const String ERR_NO_REGISTER_DATA = "No register data received.";
        public const String ERR_WRONG_PASS = "The password is incorrect.";
        public const String ERR_USER_EXISTS = "User with specified username already exists.";
        public const String ERR_ALREADY_IN = "This account was already logged in. Please relog in.";
        public const String ERR_NOT_LOGGED_IN = "You need to be logged in to do that.";
        public const String ERR_NO_RIGHTS = "You lack privilegies to do this.";
        public const String ERR_BANNED = "This account or IP address has been banned by <i>{0}</i>. Reason for this ban is <i>{1}</i>.\nBan id: <i>{2}</i>\nThis ban will expire in <i>{3}</i>.";
        public const String ERR_INVALID_DURATION = "You have specified invalid duration.";
        public const String ERR_SEARCH_FAILED = "Error occured while searching through levels.";
        public const String ERR_NO_QUERY_DATA = "No query data specified.";
        public const String ERR_USERNAME_TOO_LONG = "Your name can not be more than 20 characters long.";
        public const String ERR_PASSWORD_TOO_LONG = "Your password can not be more than 20 characters long.";
        public const String ERR_EMAIL_TOO_LONG = "Your email address can not be more than 254 characters long.";
        public const String ERR_USERNAME_INVALID = "There is an invalid character in your name. The allowed characters are a-z, A-Z, 1-9, and !#$%&()*+.:;=?@~- .";
        public const String ERR_PASSWORD_INVALID = "There is an invalid character in your password. The allowed characters are a-z, A-Z, 1-9, and !#$%&()*+.:;=?@~- .";
        public const String ERR_EMAIL_INVALID = "The provided email is in incorrect format.";
    }

    public static class StatusMessages
    {
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
        public const String NAH = "nah";
        public const String FORFEIT = "forfeit";
        public const String ONE = "1";
    }

    public static class MimeTypes
    {
        public const String MIME_TEXT_PLAIN = "text/plain";
        public const String MIME_TEXT_JSON = "text/json";
        public const String MIME_TEXT_XML = "text/xml";
    }

    // TODO - Cleanup this.
    public static class Separators
    {
        public static readonly Char[] SEPARATOR_COMMA = new Char[] { ',' };
        // Backtick.
        public const Char ARG_CHAR = '`';
        public const Char EOT_CHAR = '\x04';
        public const Char COMMA_CHAR = ',';
        public const Char PERIOD_CHAR = '.';
        public static Char[] UNDERSCORE_SEPARATOR = new Char[] { '_' };
        public static Char[] COMMA_SEPARATOR = new Char[] { ',' };
        public static Char[] ARG_SEPARATOR = new Char[] { '`' };
        public static Char[] EOT_SEPARATOR = new Char[] { EOT_CHAR };
    }

    public static class GameRooms
    {
        public const String ROOM_NONE = "none";
        public const String ROOM_MAIN = "main";
        public const String ROOM_CAMPAIGN = "campaign";
        public const String ROOM_ALLTIMEBEST = "best";
        public const String ROOM_TODAYSBEST = "best_today";
        public const String ROOM_SEARCH = "search";
    }

    public static class ValidationConstraints
    {
        public const Byte USERNAME_LENGTH = 20;
        public const Byte PASSWORD_LENGTH = 20;
        public const Byte EMAIL_LENGTH = 254;

        public const String USERNAME_PATTERN = @"^[a-zA-Z0-9 \-!#\$%&\(\)\*\+\.:;=\?@~]*$";
        public const String PASSWORD_PATTERN = @"^[a-zA-Z0-9 \-!#\$%&\(\)\*\+\.:;=\?@~]*$";
        public const String EMAIL_PATTERN = @"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})?$";
    }

    // How about more appropriate name.
    public static class Other
    {
        public static readonly DateTime UNIX_TIME = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    }
}
