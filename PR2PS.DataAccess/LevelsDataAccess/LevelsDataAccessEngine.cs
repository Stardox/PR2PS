using PR2PS.Common.DTO;
using System;

namespace PR2PS.DataAccess.LevelsDataAccess
{
    public class LevelsDataAccessEngine : ILevelsDataAccessEngine
    {
        private LevelsContext dbContext;

        public LevelsDataAccessEngine(LevelsContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Dispose()
        {
            if (this.dbContext != null)
            {
                this.dbContext.Dispose();
                this.dbContext = null;
            }
        }

        public void SaveLevel(Int64 userId, LevelDataDTO levelData)
        {
            throw new NotImplementedException();
        }
    }
}
