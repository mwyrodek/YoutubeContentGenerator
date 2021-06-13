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
    public class SimpleEngine : IEngine
    {
        
        private readonly ILogger logger;
        private readonly ILinkShortener shortener;
        private readonly IYouTubeDescriptionGenerator youTubeDescriptionGenerator;
        private readonly IWeeklySummaryGenerator summeryGenerator;
        private readonly ILoadData data;
        private readonly IEpisodeNumberHelper episodeNumberHelper;
        internal List<Episode> Episodes { get; private set; }

        public SimpleEngine(ILogger<SimpleEngine> logger, ILinkShortener shortener, IYouTubeDescriptionGenerator youTubeDescriptionGenerator, IWeeklySummaryGenerator summeryGenerator, ILoadData data, IEpisodeNumberHelper episodeNumberHelper)
        {
            this.logger = logger;
            this.shortener = shortener;
            this.youTubeDescriptionGenerator = youTubeDescriptionGenerator;
            this.summeryGenerator = summeryGenerator;
            this.data = data;
            this.episodeNumberHelper = episodeNumberHelper;
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
            youTubeDescriptionGenerator.CreateEpisodesDescription(Episodes);
            youTubeDescriptionGenerator.Save();
        }

        private void UpdateNumbers()
        {

            int num = episodeNumberHelper.GetLastEpisodeNumber();

            if(num==0)
            {
                logger.LogError("Last ep file is empty");
                return;
            }
            foreach (var episode in Episodes)
            {
                num++;
                episode.EpisodeNumber = num;

            }
#if TEST
            logger.LogInformation("TEST RUN NO WRITE OPERATIONS PERFORMED");
            logger.LogInformation($"Pretending to update last ep number to {num}");
#else
            episodeNumberHelper.UpdateLastEpisodeNumber(num);
#endif
        }

        public void GenerateWeekSummary()
        {
            summeryGenerator.CreateWeeklySummaryDescription(Episodes);
            summeryGenerator.Save();
        }
    }
}