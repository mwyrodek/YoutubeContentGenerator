using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using YCG.Models;

namespace YoutubeContentGenerator.LinkShortener
{
    public class YourlsLinkShortener : ILinkShortener
    {
        private readonly ILogger<YourlsLinkShortener> logger;
        private readonly IYourlsApi api;

        public  YourlsLinkShortener(ILogger<YourlsLinkShortener> logger, IYourlsApi api)
        {
            this.logger = logger;
            this.api = api;
        }
        
        public List<Episode> ShortenAllLinks(List<Episode> episodes)
        {
            foreach (var episode in episodes)
            {
                foreach (var article  in episode.Articles)
                {
                    try
                    {
                        article.Link = api.ShortenUrl(article.Link);
                    }
                    catch (UnauthorizedAccessException ae)
                    {
                        logger.LogWarning($"An error with authorization to link shortener skiping rest of the linkts P{ae.Message}");
                        return episodes;
                    }
                    catch (Exception e)
                    {
                        logger.LogWarning($"There was issue with url {article.Link} Error {e.Message}");
                    }
                    
                }
            }

            return episodes;
        }
    }
}