using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Google.Apis.Logging;
using Microsoft.Extensions.Logging;
using YCG.Models;
using YoutubeContentGenerator.EpisodeGenerator;

namespace YoutubeContentGenerator
{
    [ExcludeFromCodeCoverage]
    public class YouTubeDescriptionGeneratorDummy : IYouTubeDescriptionGenerator
    {
        private readonly ILogger<YouTubeDescriptionGeneratorDummy> logger;

        public YouTubeDescriptionGeneratorDummy(ILogger<YouTubeDescriptionGeneratorDummy> logger) => this.logger = logger;

        public void CreateEpisodesDescription(List<Episode> episodes)
        {
            logger.LogInformation("pretending to create episode description");
        }

        public void Save()
        {
            logger.LogInformation("Pretending to save them");
        }
    }
}