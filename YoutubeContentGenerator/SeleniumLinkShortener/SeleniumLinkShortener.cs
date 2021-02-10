using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using YCG.Models;
using YoutubeContentGenerator.Blog;
using YoutubeContentGenerator.Settings;

namespace YoutubeContentGenerator.SeleniumLinkShortener
{
    public class SeleniumLinkShortener : ILinkShortener
    {

        private readonly ILoginPage loginPage;
        private readonly IQuickLinkPage quickLinkPage;
        private readonly ILogger<SeleniumLinkShortener> logger;

        private readonly string username; 
        private readonly string password; 

        public SeleniumLinkShortener(ILoginPage loginPage, IQuickLinkPage quickLinkPage, IOptions<WordPressOptions> options, ILogger<SeleniumLinkShortener> logger)
        {
            this.loginPage = loginPage;
            this.quickLinkPage = quickLinkPage;
            this.logger = logger;
            username = options.Value.BlogLogin;
            password = options.Value.BlogPassword;
        }
        

        public List<Episode> ShortenAllLinks(List<Episode> episodes)
        {
            logger.LogTrace("Logging in to Wordpress");
            loginPage.GoTo();
            loginPage.Login(username, password);
            logger.LogTrace("user logged into to dashboard");
            quickLinkPage.GoTo();
            logger.LogTrace("user on dashboard");
            logger.LogTrace("Starting shortening links");
            
            foreach (var episode in episodes)
            {
                
                foreach (var article in episode.Articles)
                {
                    
                    quickLinkPage.GoTo();
                    var addLink = quickLinkPage.AddLink(article.Link);
                    article.Link = addLink;
                }
            }
            logger.LogTrace("All links done");
            return episodes;
        }
    }
}