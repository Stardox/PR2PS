using System;

namespace PR2PS.Web.Core.FormModels
{
    public class GetMessagesFormModel
    {
        public Int32? Start { get; set; }
        public Int32? Count { get; set; }
        public String Token { get; set; }
        public String Rand { get; set; }
    }
}
