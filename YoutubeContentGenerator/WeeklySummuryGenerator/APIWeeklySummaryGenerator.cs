using System.Collections.Generic;
using System.Linq;
using YCG.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WordPressPCL;
using WordPressPCL.Client;
using WordPressPCL.Models;
using WordPressPCL.Utility;

namespace YoutubeContentGenerator.WeeklySummuryGenerator
{
    public class APIWeeklySummaryGenerator : IWeeklySummaryGenerator
    {
        private WeeklySummaryPost post;
        private ILogger logger;
        private string username;
        private string passowrd;
        private string blogUrl;
        private WordPressClient client;


        public APIWeeklySummaryGenerator( ILogger<APIWeeklySummaryGenerator> logger, IConfiguration configuration)
        {
            this.logger = logger;
            username = configuration["Authentication:BlogLogin"];
            passowrd = configuration["Authentication:BlogPassword"];
            blogUrl =  $"{configuration["Authentication:BlogUrl"]}/wp-json/";
            client = new WordPressClient(blogUrl);
            client.AuthMethod = AuthMethod.JWTAuth;
            //todo wait
            client.RequestJWToken(username, passowrd);
        }

        public void CreateWeeklySummaryDescription(List<Episode> episodes)
        {
            post = WeeklySummaryContentGenerator.CreateWeeklySummaryContent(episodes);
        }

        public void Save()
        {
            var date = Dates.GetNextWeekSaturday();
            
            var result = client.Categories.GetAll().Result.Where(p=>p.Name == "ITea").First();
            
            var blogPost = new Post()
            {
                Title = new Title(this.post.Title),
                Content = new Content(this.post.Body),
                Status = Status.Future,
                Date = date,
                Categories = new []{result.Id}
            };
            client.Posts.Create(blogPost);
        }
    }
}