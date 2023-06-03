using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using YCG.Models;
using YoutubeContentGenerator.EpisodeGenerator;
using YoutubeContentGenerator.LoadData;
using YoutubeContentGenerator.WeeklySummuryGenerator;

[assembly:InternalsVisibleTo("YCG.Tests")]
namespace YoutubeContentGenerator.Engine
{
    public class CosmosDBEngine : IEngine
    {
        
        private readonly ILogger logger;
        private readonly ILinkShortener shortener;
        private readonly IYouTubeDescriptionGenerator youTubeDescriptionGenerator;
        private readonly IWeeklySummaryGenerator summeryGenerator;
        private readonly ILoadData data;
        private readonly IDataBaseQuery dbQuery;
        internal List<Episode> Episodes { get; private set; }

        public CosmosDBEngine(ILogger<CosmosDBEngine> logger, ILinkShortener shortener, IYouTubeDescriptionGenerator youTubeDescriptionGenerator, IWeeklySummaryGenerator summeryGenerator, ILoadData data, IDataBaseQuery dbQuery)
        {
            this.logger = logger;
            this.shortener = shortener;
            this.youTubeDescriptionGenerator = youTubeDescriptionGenerator;
            this.summeryGenerator = summeryGenerator;
            this.data = data;
            this.dbQuery = dbQuery;
        }
        
        public void LoadData()
        {
            Episodes = data.Execute();
        }

        public void GenerateLinks()
        {
            Episodes = shortener.ShortenAllLinks(Episodes);
        }

        public void GenerateDescription()
        {
            UpdateNumbers();
            SaveToDb();
            youTubeDescriptionGenerator.CreateEpisodesDescription(Episodes);
            youTubeDescriptionGenerator.Save();
        }
        
        public void GenerateSpecialEpisodeDescription(SpecialEpisodeType type)
        {
            youTubeDescriptionGenerator.CreateSpecialEpisodesDescription(type);
            youTubeDescriptionGenerator.Save();
        }

        private void SaveToDb()
        {
#if TEST
            logger.LogInformation("TEST RUN NO WRITE OPERATIONS PERFORMED");
            
#else
            dbQuery.PushEpisode(Episodes);
#endif
        }

        private void UpdateNumbers()
        {

            int num = dbQuery.GetLastEpisodeNumber();

            if(num<1)
            {
                logger.LogError("there was error with geting episde number");
                return;
            }
            foreach (var episode in Episodes)
            {
                num++;
                episode.EpisodeNumber = num;

            }

        }

        public void GenerateWeekSummary()
        {
            summeryGenerator.CreateWeeklySummaryDescription(Episodes);
            summeryGenerator.Save();
        }
    }
}