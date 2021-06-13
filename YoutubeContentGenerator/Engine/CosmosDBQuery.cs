using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YCG.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using YoutubeContentGenerator.Settings;

namespace YoutubeContentGenerator.Engine
{
    public class CosmosDbQuery :IDataBaseQuery
    {
     
        private readonly ILogger logger;
        private readonly IOptions<CosmosDB> options;

        public CosmosDbQuery(ILogger<CosmosDbQuery> logger, IOptions<CosmosDB> options)
        {
            this.logger = logger;
            this.options = options;
     
        }

        public int GetLastEpisodeNumber()
        {
            using var client = new CosmosClient(options.Value.Uri, options.Value.PrimaryKey);
            var container = client.GetContainer("IteaMorningArticlesDatabase","Episode");
            var sql = "SELECT top 1 c.episodeNumber FROM c order by c.episodeNumber desc";
            var iterator = container.GetItemQueryIterator<dynamic>(sql);
            var task =  iterator.ReadNextAsync();
            task.Wait();
            var page = task.Result;

            var number = page.First().episodeNumber;
            return (int) number;
        }

        public void PushEpisode(List<Episode> episodes)
        {
            var cosmosOption = new CosmosClientOptions()
            {
                SerializerOptions = new CosmosSerializationOptions()
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            };
            using var client = new CosmosClient(options.Value.Uri, options.Value.PrimaryKey, cosmosOption);
            var container = client.GetContainer("IteaMorningArticlesDatabase","Episode");
            foreach (var episode in episodes)
            {
                var itemAsync = container.CreateItemAsync<Episode>(episode);
                itemAsync.Wait();
                var item = itemAsync.Result;
                Console.WriteLine(item.Resource.EpisodeNumber);
                
            }
            
        }
    }
}