using System;
using static PR2PS.LevelImporter.Core.Enums;

namespace PR2PS.LevelImporter.Models
{
    public class ImportProgress
    {
        public ProgressType ProgressType { get; set; }
        public String Message { get; set; }
        public LevelModel LevelModel { get; set; }
    }
}
