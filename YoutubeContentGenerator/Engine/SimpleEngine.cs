using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using YCG.Models;
using YoutubeContentGenerator.EpisodeGenerator;
using YoutubeContentGenerator.LoadData;
using YoutubeContentGenerator.WeeklySummuryGenerator;

namespace YoutubeContentGenerator.Engine
{
    public class SimpleEngine : IEngine
    {
        
        private readonly ILogger logger;
        private readonly ILinkShortener shortener;
        private readonly IYouTubeDescriptionGenerator youTubeDescriptionGenerator;
        private readonly IWeeklySummaryGenerator summeryGenerator;
        private readonly ILoadData data;
        private readonly IConfiguration configuration;
        private List<Episode> episodes;

        public SimpleEngine(ILogger<SimpleEngine> logger, ILinkShortener shortener, IYouTubeDescriptionGenerator youTubeDescriptionGenerator, IWeeklySummaryGenerator summeryGenerator, ILoadData data, IConfiguration configuration)
        {
            this.logger = logger;
            this.shortener = shortener;
            this.youTubeDescriptionGenerator = youTubeDescriptionGenerator;
            this.summeryGenerator = summeryGenerator;
            this.data = data;
            this.configuration = configuration;
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
        private void UpdateNumbers()
        {

            int num = 0;
            using (StreamReader sr = new StreamReader(configuration["Defaults:DefaultLastEpNumberFile"]))
            {
                string line;
                // Read and display lines from the file until the end of
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    num = int.Parse(line);
                }
            }
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
            using (StreamWriter writer = new StreamWriter(configuration["Defaults:DefaultLastEpNumberFile"], false))
            {
                writer.Write(num);
            }
#endif
        }

        public void GenerateWeekSummary()
        {
            summeryGenerator.CreateWeeklySummaryDescription(episodes);
            summeryGenerator.Save();
        }
    }
}