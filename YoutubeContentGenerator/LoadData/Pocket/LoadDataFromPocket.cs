using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using YCG.Models;
using YoutubeContentGenerator.Settings;

namespace YoutubeContentGenerator.LoadData.Pocket
{
    public class LoadDataFromPocket : ILoadData
    {
     //old categories
     //private List<string> tags = new List<string>{ "testing", "dev", "busisness", "joker" };
     //todo move it to config
        private List<string> tags;
        private readonly ILogger logger;
        
        private readonly IPocketConector pocketConector;
        private PocketOptions options;

        public LoadDataFromPocket(ILogger<LoadDataFromPocket> logger, IOptions<PocketOptions> options, IPocketConector pocketConector)
        {
            this.logger = logger;
            this.options = options.Value; 
            tags = options.Value.Tags;
            
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

        
        /// <summary>
        /// lests asume season is week = 
        /// </summary>
        /// <returns></returns>
        private List<Episode> CreateSeason()
        {
            var list = new List<Episode>();
            for (int i = 0; i < int.Parse(options.SeasonLength); i++)
            {
                list.Add(CreateEpisode());
            }
            return list;
        }
    }
}
