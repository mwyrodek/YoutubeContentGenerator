using System.Collections.Generic;
using System.Linq;
using YCG.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WordPressPCL;
using WordPressPCL.Models;
using YoutubeContentGenerator.Settings;

namespace YoutubeContentGenerator.WeeklySummuryGenerator
{
    public class APIWeeklySummaryGenerator : IWeeklySummaryGenerator
    {
        private WeeklySummaryPost post;
        private ILogger logger;
        private string username;
        private string passowrd;
        private string blogUrl;
        private string category;
        private WordPressClient client;


        public APIWeeklySummaryGenerator( ILogger<APIWeeklySummaryGenerator> logger, IOptions<WordPressOptions> options)
        {
            this.logger = logger;
            username = options.Value.BlogLogin;
            passowrd = options.Value.BlogPassword;
            category = options.Value.BlogCategoryName;
            blogUrl =  $"{options.Value.BlogUrl}/wp-json/";
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
            //todo to chyba tez trzeba wyciagnac do configa
            var result = client.Categories.GetAll().Result.Where(p=>p.Name == category).First();
            
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