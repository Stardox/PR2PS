using System;

namespace PR2PS.Web.Core.FormModels
{
    public class DeleteOrReportMessageFormModel
    {
        public Int64? Message_Id { get; set; }
        public String Token { get; set; }
        public String Rand { get; set; }
    }
}
