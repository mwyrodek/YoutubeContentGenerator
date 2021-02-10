using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Options;
using YCG.Models;
using YoutubeContentGenerator.Settings;

namespace YoutubeContentGenerator.LoadData.Pocket
{
    public class LoadDataFromPocket : ILoadData
    {
        private readonly List<string> tags;
        private readonly ILogger logger;
        
        private readonly IPocketConector pocketConnector;
        private readonly PocketOptions options;

        public LoadDataFromPocket(ILogger<LoadDataFromPocket> logger, IOptions<PocketOptions> options, IPocketConector pocketConnector)
        {
            this.logger = logger;
            this.options = options.Value; 
            tags = options.Value.Tags;
            
            this.pocketConnector = pocketConnector;
        }

        public List<Episode> Execute()
        {
            ValidateSeasonLength();
            ValidateTags();
            return CreateSeason();
        }

        private void ValidateTags()
        {
            if(tags.Count<1) throw new InvalidOperationException ("At least one Tag is required");
        }

        private void ValidateSeasonLength()
        {
            if (options.SeasonLength <= 0) throw new InvalidOperationException ("Season Length has to be greater than 0");
        }

        private Episode CreateEpisode()
        {
            logger.LogTrace("Creating Episode");
            var episode =new Episode();
            foreach (var tag in tags)
            {
                    var article = pocketConnector.MoveArticleFromPocketByTag(tag);
                    if (article != null)
                    {
                        episode.Articles.Add(article);    
                    }
                    else
                    {
                        logger.LogWarning($"No more aricles for tag {tag}.");
                    }
                    
                
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
            this.logger.LogTrace($"Creating Season of {options.SeasonLength} epsiodes.");
            var list = new List<Episode>();
            for (var i = 0; i < options.SeasonLength; i++)
            {
                var epsiode = CreateEpisode();
                if (epsiode.Articles.Any())
                {
                    list.Add(epsiode);
                }
                else
                {
                    logger.LogWarning($"No article for any tag  skiping episode creation on {i} episode");
                }

            }

            this.logger.LogTrace("Season Created");
            return list;
        }
    }
}
