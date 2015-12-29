using System;

namespace PR2PS.Web.Core.FormModels
{
    public class ChangePassFormModel
    {
        public String Name { get; set; }
        public String Old_Pass { get; set; }
        public String New_Pass { get; set; }
        public String Token { get; set; }
        public String Rand { get; set; }
    }
}
