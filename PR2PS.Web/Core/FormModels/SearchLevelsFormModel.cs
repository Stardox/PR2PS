using System;
using static PR2PS.Common.Enums;

namespace PR2PS.Web.Core.FormModels
{
    public class SearchLevelsFormModel
    {
        public String Search_Str { get; set; }
        public SearchMode Mode { get; set; }
        public SearchOrder Order { get; set; }
        public SearchDirection Dir { get; set; }
        public Int16? Page { get; set; }
        public String Token { get; set; }
        public String Rand { get; set; }
    }
}
