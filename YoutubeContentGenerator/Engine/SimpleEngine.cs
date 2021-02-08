using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using YCG.Models;
using YoutubeContentGenerator.EpisodeGenerator;
using YoutubeContentGenerator.LoadData;
using YoutubeContentGenerator.Settings;
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
        internal List<Episode> episodes { get; private set; }
        private readonly DefaultsOptions options;

        public SimpleEngine(ILogger<SimpleEngine> logger, ILinkShortener shortener, IYouTubeDescriptionGenerator youTubeDescriptionGenerator, IWeeklySummaryGenerator summeryGenerator, ILoadData data, IOptions<DefaultsOptions> options, IEpisodeNumberHelper episodeNumberHelper)
        {
            this.logger = logger;
            this.shortener = shortener;
            this.youTubeDescriptionGenerator = youTubeDescriptionGenerator;
            this.summeryGenerator = summeryGenerator;
            this.data = data;
            this.options = options.Value;
            this.episodeNumberHelper = episodeNumberHelper;
        }
        
        public void LoadData()
        {
            episodes = data.Execute();
        }

        public void GenerateLinks()
        {
            episodes = shortener.ShortenAllLinks(episodes);
        }

        public void GenerateDescription()
        {
            UpdateNumbers();
            youTubeDescriptionGenerator.CreateEpisodesDescription(episodes);
            youTubeDescriptionGenerator.Save();
        }

        //todo move it to file updater
        //todo i don't know what this method does anymore and i am afraid to move it
        private void UpdateNumbers()
        {

            int num = episodeNumberHelper.GetLastEpisodeNumber();

            if(num==0)
            {
                logger.LogError("Last ep file is empty");
                return;
            }
            foreach (var episode in episodes)
            {
                num++;
                episode.EpisodeNum = num;

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
            summeryGenerator.CreateWeeklySummaryDescription(episodes);
            summeryGenerator.Save();
        }
    }
}