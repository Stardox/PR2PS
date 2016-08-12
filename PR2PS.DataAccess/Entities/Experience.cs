using System;

namespace PR2PS.DataAccess.Entities
{
    public class Experience : BaseEntity
    {
        public Int64 ExpCurrent { get; set; }
        public Int64 ExpLevelUp { get; set; }

        public Experience()
        {
            this.ExpCurrent = 0;
            this.ExpLevelUp = 1;
        }
    }
}
