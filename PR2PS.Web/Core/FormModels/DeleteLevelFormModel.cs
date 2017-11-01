using System;

namespace PR2PS.Web.Core.FormModels
{
    public class DeleteLevelFormModel
    {
        public Int64? Level_Id { get; set; }
        public String Token { get; set; }
        public String Rand { get; set; }
    }
}
