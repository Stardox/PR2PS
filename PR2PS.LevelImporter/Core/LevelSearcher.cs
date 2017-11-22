using PR2PS.Common.Constants;
using PR2PS.Common.Extensions;
using PR2PS.LevelImporter.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PR2PS.LevelImporter.Core
{
    public class LevelSearcher
    {
        public Boolean IsBusy { get; set; }

        public async Task<List<LevelResult>> DoSearch(String term, String mode, String order, String direction, String page)
        {
            try
            {
                this.IsBusy = true;

                using (WebClient downloader = new WebClient())
                {
                    NameValueCollection data = new NameValueCollection
                    {
                        { "search_str", term },
                        { "mode", mode.ToLower() },
                        { "order", order.ToLower() },
                        { "dir", direction.ToLower() },
                        { "page", page }
                    };

                    Byte[] response = await downloader.UploadValuesTaskAsync("http://pr2hub.com/search_levels.php", data);
                    String result = Encoding.UTF8.GetString(response);
                    List<LevelResult> results = this.Parse(result);

                    return results;
                }
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private List<LevelResult> Parse(String data)
        {
            List<LevelResult> results = new List<LevelResult>();

            if (String.IsNullOrEmpty(data))
            {
                return results;
            }

            Dictionary<String, String> attributes = data
                .Split(Separators.AMPERSAND_SEPARATOR, StringSplitOptions.RemoveEmptyEntries)
                .Where(g => g.Contains(Separators.EQ_CHAR))
                .ToDictionary(g => g.Split(Separators.EQ_CHAR)[0], g => g.Split(Separators.EQ_CHAR)[1]);

            for (Byte idx = 0; idx <= 5; idx++)
            {
                String levelIdTmp;
                String versionTmp;
                String authorTmp;
                String gameModeTmp;
                String titleTmp;

                if (attributes.TryGetValue(LevelListKeys.LEVEL_ID + idx, out levelIdTmp)
                    && attributes.TryGetValue(LevelListKeys.VERSION + idx, out versionTmp)
                    && attributes.TryGetValue(LevelListKeys.USERNAME + idx, out authorTmp)
                    && attributes.TryGetValue(LevelListKeys.TYPE + idx, out gameModeTmp)
                    && attributes.TryGetValue(LevelListKeys.TITLE + idx, out titleTmp))
                {
                    results.Add(new LevelResult
                    {
                        LevelId = levelIdTmp,
                        Version = versionTmp,
                        Author = authorTmp.ToUrlDecodedString(),
                        GameMode = this.GetFullGameMode(gameModeTmp),
                        Title = titleTmp.ToUrlDecodedString()
                    });
                }
            }

            return results;
        }

        private String GetFullGameMode(String input)
        {
            switch (input)
            {
                case "r": return "Race";
                case "d": return "Deathmatch";
                case "o": return "Objective";
                case "e": return "Egg";
                default: return "Unknown";
            }
        }
    }
}
