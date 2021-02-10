using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Authentication;
using Microsoft.Extensions.Logging;
using WordPressPCL;
using WordPressPCL.Models;
using YCG.Models;

namespace YoutubeContentGenerator.WeeklySummuryGenerator.WordPressWrapper
{

    //this is wrapper for external library which doesnt have interfaces
    //By desging this class wont be covered by unit test
    //Also yes this is overdesgined solution as hell :)
    [ExcludeFromCodeCoverage]
    public class WordPressClientWrapper : IWordPressClientWrapper
    {
        private WordPressClient client;
        private readonly ILogger logger;
        public WordPressClientWrapper(ILogger<WordPressClientWrapper> logger)
        {
            this.logger = logger;
        }
        public IWordPressClientWrapper CreateClient(string blogUrl)
        {
            logger.LogTrace($"creating client for address {blogUrl}");
            client = new WordPressClient(blogUrl);
            logger.LogTrace($"word press client created");
            return this;
        }

        public IWordPressClientWrapper Authenticate(string username, string password)
        {
            logger.LogTrace("Requesting JWT token");
            client.AuthMethod = AuthMethod.JWTAuth;
            client.RequestJWToken(username, password);
            logger.LogInformation($"Checking if token is valid {client.IsValidJWToken().Result}");
            var isValidJwToken = client.IsValidJWToken();
            isValidJwToken.Wait();
            if (!isValidJwToken.Result)
            {
                throw new AuthenticationException("Token is not valid");
            }
            logger.LogTrace("Token recived and validated");
            return this;
        }

        public IWordPressClientWrapper Post(WeeklySummaryPost post, string category, DateTime publishDate)
        {
            var blogPost = new Post()
            {
                Title = new Title(post.Title),
                Content = new Content(post.Body),
                Status = Status.Future,
                Date = publishDate,
                Categories = new []{GetCategory(category).Id}
            };
            logger.LogTrace($"Scheduleing post for date {publishDate}");
            client.Posts.Create(blogPost);
            logger.LogTrace($"Post Scheduled");
            return this;
        }

        private Category GetCategory(string category)
        {
            logger.LogTrace($"geting wordpress category with name {category}");
            var task = client.Categories.GetAll();
            task.Wait();
            return task.Result.First(p=>p.Name == category);
        }
    }
}