using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PocketSharp;
using PocketSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YCG.Models;

namespace YoutubeContentGenerator.LoadData.Pocket
{
    public class PocketConector : IPocketConector
    {
       
       private readonly ILogger logger;
        private readonly IConfiguration configuration;
        private readonly IPocketClient pocketClient;

        public PocketConector(ILogger<PocketConector> logger, IConfiguration configuration, IPocketFactory pocketFactory)
        {
            this.logger = logger;
            this.configuration = configuration;
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
            var item = items.Result.First();
            var id = item.ID;
            logger.LogInformation($"Article ID :{id}");
            var article = PocketMapper.Map(item);
#if !TestRun
            pocketClient.Archive(id);
#endif
            return article;
        }



        //getpocketclinet


        //archivearticle
    }
}
