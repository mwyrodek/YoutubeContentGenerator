using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PocketSharp;
using PocketSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using YCG.Models;
using YoutubeContentGenerator.Settings;

namespace YoutubeContentGenerator.LoadData.Pocket
{
    public class PocketConector : IPocketConector
    {
       
       private readonly ILogger logger;
       private readonly IPocketClient pocketClient;

        public PocketConector(ILogger<PocketConector> logger, IPocketFactory pocketFactory)
        {
            this.logger = logger;
            this.pocketClient = pocketFactory.CreatePocketClient();
        }

        /// <summary>
        /// Moves Article from pocket to list
        /// </summary>
        /// <returns></returns>
        public Article MoveArticleFromPocketByTag(string tag)
        {
            logger.LogDebug($"Starting downloading article for tag:{tag}");
            var items = pocketClient.Get(state: State.unread, tag: tag, sort: Sort.newest);
            items.Wait();
            
            var item = items.Result.FirstOrDefault();
            if (item == null)
            {
                logger.LogWarning($"Tag {tag} has no more articles");
                return null;
            }
            var id = item.ID;
            logger.LogInformation($"Article ID :{id}");
            var article = PocketMapper.Map(item);
#if !TEST
            pocketClient.Archive(id);
#endif
            return article;
        }



        //getpocketclinet


        //archivearticle
    }
}
