using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using YCG.Models;

namespace YoutubeContentGenerator.LoadData.Pocket
{
    public class LoadDataFromPocket : ILoadData
    {
     //old categories
     //private List<string> tags = new List<string>{ "testing", "dev", "busisness", "joker" };
        private List<string> tags = new List<string>{ "hard", "soft"};
        private readonly ILogger logger;
        private readonly IConfiguration configuration;
        private readonly IPocketConector pocketConector;

        public LoadDataFromPocket(ILogger<LoadDataFromPocket> logger, IConfiguration configuration, IPocketConector pocketConector)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.pocketConector = pocketConector;
        }

        public List<Episode> Execute()
        {
            return CreateSeason();
        }

        private Episode CreateEpisode()
        {
            logger.LogTrace("Creating Episode");
            var episode =new Episode();
            foreach (var tag in tags)
            {
                    var article = pocketConector.MoveArticleFromPocketByTag(tag);
                    episode.Articles.Add(article);
                
            }
            logger.LogTrace("Episode Done");
            return episode;
        }

        //todo parametrise amount of episodes in season
        /// <summary>
        /// lests asume season is week = 
        /// </summary>
        /// <returns></returns>
        private List<Episode> CreateSeason()
        {
            var list = new List<Episode>();
            for (int i = 0; i < int.Parse(configuration["Defaults:SeasonLength"]); i++)
            {
                list.Add(CreateEpisode());
            }
            return list;
        }
    }
}
