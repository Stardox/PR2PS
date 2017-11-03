using System;

namespace PR2PS.Web.Core.Management
{
    public sealed class ConfigurationManager
    {
        #region Singleton

        private static readonly ConfigurationManager instance = new ConfigurationManager();

        public static ConfigurationManager Instance
        {
            get { return instance; }
        }

        static ConfigurationManager() { }

        private ConfigurationManager() { }

        #endregion

        public String SearchUrl { get; set; }
    }
}
