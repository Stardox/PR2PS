using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR2PS.DataAccess.Engine
{
    public class CommandEngine : ICommandEngine
    {
        private DatabaseContext dbContext;

        public CommandEngine(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
