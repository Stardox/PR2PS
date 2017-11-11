using PR2PS.Common.Constants;
using PR2PS.Common.Cryptography;
using PR2PS.Common.Extensions;
using System;
using System.Globalization;
using System.Text;
using static PR2PS.Common.Enums;

namespace PR2PS.Common.DTO
{
    public class LevelDataDTO
    {
        public Int64 Id { get; set; }
        public Int32 Version { get; set; }
        public Int64 UserId { get; set; }
        public String Title { get; set; }
        public String Note { get; set; }
        public Boolean Live { get; set; }
        public GameMode GameMode { get; set; }
        public Byte MinLevel { get; set; }
        public Single Gravity { get; set; }
        public Int16 Song { get; set; }
        public String Items { get; set; }
        public Int16 MaxTime { get; set; }
        public String Hash { get; set; }
        public String PassHash { get; set; }
        public String Data { get; set; }
        public Byte CowboyChance { get; set; }
        public DateTime SubmittedDate { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            // TODO - Rewrite this shit. Use dictionary or something.
            // Yes, I know that string interpolation exists, but I dont like it.
            sb.AppendFormat(
                "levelID={0}&version={1}&user_id={2}&credits={3}&cowboyChance={4}&title={5}&time={6}&note={7}&min_level={8}&song={9}&gravity={10}&max_time={11}&has_pass={12}&live={13}&items={14}&gameMode={15}&data={16}",
                this.Id,
                this.Version,
                this.UserId,
                String.Empty,
                this.CowboyChance,
                this.Title.ToUrlEncodedString(),
                this.SubmittedDate.GetSecondsSinceUnixTime(),
                this.Note.ToUrlEncodedString(),
                this.MinLevel,
                this.Song,
                this.Gravity.ToString(StringFormat.DECIMAL_TWO, CultureInfo.InvariantCulture),
                this.MaxTime,
                !String.IsNullOrEmpty(this.PassHash),
                Convert.ToInt32(this.Live),
                this.Items,
                this.GameMode.GetEnumDescription(),
                this.Data);

            using (MD5Wrapper md5 = new MD5Wrapper())
            {
                String hash = md5.GetHashedString(String.Concat(this.Version.ToString(), this.Id.ToString(), sb.ToString(), Pepper.LEVEL_LOAD));
                sb.Append(hash);
            }

            return sb.ToString();
        }
    }
}
