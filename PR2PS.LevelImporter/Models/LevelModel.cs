using System;
using System.IO;
using static PR2PS.LevelImporter.Core.Enums;

namespace PR2PS.LevelImporter.Models
{
    public class LevelModel
    {
        public UserModel User { get; set; }
        public SourceType SourceType { get; private set; }
        public String Location { get; private set; }
        public String Render { get; private set; }

        public LevelModel(UserModel user, String path)
        {
            this.User = user;
            this.SourceType = SourceType.Local;
            this.Location = path;
            this.Render = String.Concat("Local ", Path.GetFileName(path), " -> ", user.Username);
        }

        public LevelModel(UserModel user, String id, String version)
        {
            this.User = user;
            this.SourceType = SourceType.Remote;
            this.Location = String.Format("http://pr2hub.com/levels/{0}.txt{1}", id, String.IsNullOrEmpty(version) ? String.Empty : String.Concat("?version=", version));

            String tmp = String.Format("{0}{1}", id, String.IsNullOrEmpty(version) ? " (latest)" : String.Concat("_", version));
            this.Render = String.Concat("Remote ", tmp, " -> ", user.Username);
        }
    }
}
