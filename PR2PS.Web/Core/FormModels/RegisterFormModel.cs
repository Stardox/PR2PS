using System;

namespace PR2PS.Web.Core.FormModels
{
    public class RegisterFormModel
    {
        public String Name { get; set; }
        public String Password { get; set; }
        public String Email { get; set; }
        public String Token { get; set; }
        public String Rand { get; set; }
    }
}
