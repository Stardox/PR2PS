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
        public const String ERR_NO_SUCH_USER = "No such user exists.";
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
        public const String ERR_NO_RIGHTS = "You lack privileges to do this.";
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
        public const String ERR_NOT_IMPLEMENTED = "This feature is not implemented yet.";
        public const String ERR_MESSAGE_NOT_FOUND = "Could not find such message.";
        public const String ERR_NO_MESSAGES = "You have no messages to delete.";
        public const String ERR_ADMINS_ARE_ABSOLUTE = "Administrators are absolute!";
        public const String ERR_NO_LEVEL_DATA = "No level data received to process.";
        public const String ERR_NO_LEVEL_TITLE = "Your level needs to have title.";
        public const String ERR_INVALID_GAME_MODE = "The game mode of your level is not valid.";
        public const String ERR_NO_SUCH_LEVEL = "Specified level can not be found or is deleted.";
        public const String ERR_NO_VERSION = "Failed to load version of the given level.";
        public const String ERR_UNSUPPORTED_SEARCH_ORDER = "Selected search order is not supported.";
        public const String ERR_SEARCH_TERM_TOO_SHORT = "Search term has to be at least 3 characters long.";
        public const String ERR_INVALID_RATING = "Invalid level rating value.";
        public const String ERR_ALREADY_VOTED = "You have already voted on this level. You can vote on it again in a week.";
        public const String ERR_NO_LEVEL_ID = "No level id was given.";
        public const String ERR_INVALID_ITEMS = "Level contains invalid item set.";
    }

    public static class StatusKeys
    {
        public const String ERROR = "error";
        public const String MESSAGE = "message";
        public const String SUCCESS = "success";
        public const String USERNAME = "user_name";
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
        public const String NONE = "none";
        public const String SERVER_OPEN = "open";
        public const String SERVER_DOWN = "down";
        public const String NAH = "nah";
        public const String FORFEIT = "forfeit";
        public const String ONE = "1";
        public const String TRUE = "true";
        public const String PASSWORD_CHANGED = "The password has been changed succesfully!";
        public const String MESSAGE_SENT = "Your message was sent succesfully!";
        public const String SAVE_SUCCESSFUL = "The save was successful.";
        public const String VOTE_CAST = "Thank you for voting! Your vote of {0} changed the average rating from {1} to {2}.";
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
        public const Char SPACE_CHAR = ' ';
        public const Char PLUS_CHAR = '+';
        public static Char[] UNDERSCORE_SEPARATOR = new Char[] { '_' };
        public static Char[] COMMA_SEPARATOR = new Char[] { ',' };
        public static Char[] ARG_SEPARATOR = new Char[] { '`' };
        public static Char[] EOT_SEPARATOR = new Char[] { EOT_CHAR };
        public const String ARG_STR = "`";
        public const Char EQ_CHAR = '=';
        public const String AMPERSAND = "&";
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

    public static class GameModes
    {
        public const String UNKNOWN = "unknown";
        public const String RACE = "race";
        public const String DEATHMATCH = "deathmatch";
        public const String EGG = "egg";
        public const String OBJECTIVE = "objective";
    }

    public static class ValidationConstraints
    {
        public const Byte USERNAME_LENGTH = 20;
        public const Byte PASSWORD_LENGTH = 20;
        public const Byte EMAIL_LENGTH = 254;

        public const String USERNAME_PATTERN = @"^[a-zA-Z0-9 \-!#\$%&\(\)\*\+\.:;=\?@~]*$";
        public const String PASSWORD_PATTERN = @"^[a-zA-Z0-9 \-!#\$%&\(\)\*\+\.:;=\?@~]*$";
        public const String EMAIL_PATTERN = @"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})?$";
        public const String LEVEL_ITEMS_PATTERN = @"^[1-9](`[1-9])*$";
    }

    public static class UserGroup
    {
        public const Byte GUEST = 0;
        public const Byte MEMBER = 1;
        public const Byte MODERATOR = 2;
        public const Byte ADMINISTRATOR = 3;
    }

    // How about more appropriate name.
    public static class Other
    {
        public static readonly DateTime UNIX_TIME = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public const String DATE_FORMAT = "dd/MMM/yyyy";
    }

    public static class ConnectionStringKeys
    {
        public const String PR2_MAIN_DB = "PR2MainDB";
        public const String PR2_LEVELS_DB = "PR2LevelsDB";
    }

    public static class Pepper
    {
        public const String LIST_OF_LEVELS = "984cn98c54$";
        public const String LEVEL_DATA = "0kg4%dsw";
    }

    public static class StringFormat
    {
        public const String HEX_LOWERCASE = "x2";
        public const String DECIMAL_TWO = "0.##";
    }
}
