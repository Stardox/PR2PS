using System;

namespace PR2PS.LevelImporter.Models
{
    public class UserModel
    {
        public Int64 Id { get; set; }
        public String Username { get; set; }

        public override String ToString()
        {
            return String.Format("{0} ({1})", this.Username, this.Id);
        }
    }
}
