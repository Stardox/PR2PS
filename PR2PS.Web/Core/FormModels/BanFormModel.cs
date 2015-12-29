using System;

namespace PR2PS.Web.Core.FormModels
{
    public class BanFormModel
    {
        public String Banned_Name { get; set; }
        public String Duration { get; set; }
        public String Reason { get; set; }
        public String Record { get; set; }
        public String Token { get; set; }
        public String Rand { get; set; }

        public Boolean IsIPBan { get; set; }
    }
}
