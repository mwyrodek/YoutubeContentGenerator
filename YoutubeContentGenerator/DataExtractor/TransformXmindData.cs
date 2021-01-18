using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using YCG.Models;
using YCG.Tests.Generator;

[assembly: InternalsVisibleTo("YCG.Tests")]
namespace YoutubeContentGenerator.DataExtractor
{
    public class TransformXmindData : ITransformData
    {
        private readonly ILogger logger;
        public TransformXmindData(ILogger<TransformXmindData> logger)
        {
            this.logger = logger;
        }
        public List<Episode> TransformJsonToEpisodes(string jsonObject)
        {
            var deserializedClass = DeserializeJson(jsonObject);

            //episodes
            var episodeListRaw = deserializedClass.First().rootTopic.children.attached;
            //article 
            var episodes = new List<Episode>();
            foreach (var episodeRaw in episodeListRaw)
            {
                var episode = MapEpisode(episodeRaw);

                if (episode.Articles.Count > 0)
                {
                    episodes.Add(episode);    
                }
                
            }
            return episodes;
        }


        internal List<XmindRoot> DeserializeJson(string jsonObject)
        {
            return JsonConvert.DeserializeObject<List<XmindRoot>>(jsonObject);
        }

        private Episode MapEpisode(Attached episodeRaw)
        {
            var episode = new Episode();
            foreach (var articleRaw in episodeRaw.children.attached)
            {
                if (articleRaw.children?.attached.First().children != null)
                {
                    var article = new Article
                    {
                        Title = articleRaw.children.attached.First().title,
                        Link = articleRaw.children.attached.First().children.attached.First().title
                    };
                    logger.LogInformation($"adding article {article.Title}");
                    episode.Articles.Add(article);
                }
                else
                {
                    this.logger.LogError("Article is Missing");
                }

            }

            return episode;
        }
    }
}