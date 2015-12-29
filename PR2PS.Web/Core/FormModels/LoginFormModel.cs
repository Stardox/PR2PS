using System;

namespace PR2PS.Web.Core.FormModels
{
    public class LoginFormModel
    {
        public String I { get; set; }
        public String Version { get; set; }
        public String Token { get; set; }
        public String Rand { get; set; }
    }
}
