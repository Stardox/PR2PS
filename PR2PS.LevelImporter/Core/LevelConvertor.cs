using PR2PS.DataAccess.Entities;
using PR2PS.LevelImporter.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using static PR2PS.LevelImporter.Core.Enums;

namespace PR2PS.LevelImporter.Core
{
    public class LevelConvertor : IDisposable
    {
        private WebClient downloader;

        public Boolean IsBusy { get; private set; }

        public LevelConvertor()
        {
            this.downloader = new WebClient();
        } 

        public async Task<List<Level>> GetAndConvert(List<LevelModel> items, Action<ImportProgress> progress)
        {
            try
            {
                this.IsBusy = true;
                List<Level> results = new List<Level>();

                foreach (LevelModel levMeta in items)
                {
                    // Try to obtain level data either from disk or remotely.
                    String levelData;
                    try
                    {
                        progress?.Invoke(new ImportProgress
                        {
                            ProgressType = ProgressType.Info,
                            Message = String.Format("'{0}' - Loading level data...", levMeta.Render),
                            LevelModel = levMeta
                        });

                        levelData = await this.GetLevelData(levMeta);

                        progress?.Invoke(new ImportProgress
                        {
                            ProgressType = ProgressType.Info,
                            Message = String.Format("'{0}' - Loading of level data has succeeded.", levMeta.Render),
                            LevelModel = levMeta
                        });
                    }
                    catch
                    {
                        progress?.Invoke(new ImportProgress
                        {
                            ProgressType = ProgressType.Warning,
                            Message = String.Format("'{0}' - Loading of level data has failed with error.", levMeta.Render),
                            LevelModel = levMeta
                        });

                        continue;
                    }

                    // Next up, materialize the entity from string data.
                    Level level = null;
                    try
                    {
                        progress?.Invoke(new ImportProgress
                        {
                            ProgressType = ProgressType.Info,
                            Message = String.Format("'{0}' - Materializing...", levMeta.Render),
                            LevelModel = levMeta
                        });

                        level = Level.FromString(levelData, levMeta.User.Id);
                        if (level == null)
                        {
                            progress?.Invoke(new ImportProgress
                            {
                                ProgressType = ProgressType.Warning,
                                Message = String.Format("'{0}' - Materializing has failed.", levMeta.Render),
                                LevelModel = levMeta
                            });

                            continue;
                        }

                        progress?.Invoke(new ImportProgress
                        {
                            ProgressType = ProgressType.Info,
                            Message = String.Format("'{0}' - Materializing has succeeded.", levMeta.Render),
                            LevelModel = levMeta
                        });
                    }
                    catch
                    {
                        progress?.Invoke(new ImportProgress
                        {
                            ProgressType = ProgressType.Warning,
                            Message = String.Format("'{0}' - Materializing has failed with error.", levMeta.Render),
                            LevelModel = levMeta
                        });

                        continue;
                    }

                    results.Add(level);
                }

                return results;
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private Task<String> GetLevelData(LevelModel level)
        {
            if (level.SourceType == SourceType.Local)
            {
                return Task.Run(() => File.ReadAllText(level.Location));
            }
            else if (level.SourceType == SourceType.Remote)
            {
                return this.downloader.DownloadStringTaskAsync(level.Location);
            }
            else
            {
                throw new InvalidOperationException("Unsupported source type.");
            }
        }

        public void Dispose()
        {
            if (this.downloader != null)
            {
                this.downloader.Dispose();
            }
        }
    }
}
