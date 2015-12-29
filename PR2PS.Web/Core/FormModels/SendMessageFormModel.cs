using System;

namespace PR2PS.Web.Core.FormModels
{
    public class SendMessageFormModel
    {
        public String To_Name { get; set; }
        public String Message { get; set; }
        public String Token { get; set; }
        public String Rand { get; set; }
    }
}
