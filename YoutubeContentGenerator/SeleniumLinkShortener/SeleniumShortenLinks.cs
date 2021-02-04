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
    public class SeleniumShortenLinks : ILinkShortener
    {

        private readonly ILoginPage loginPage;
        private readonly IQuickLinkPage quickLinkPage;
        private readonly ILogger<SeleniumShortenLinks> logger;
        //todo move it to config
        private string username; 
        private string passowrd; 

        public SeleniumShortenLinks(ILoginPage loginPage, IQuickLinkPage quickLinkPage, IOptions<WordPressOptions> options, ILogger<SeleniumShortenLinks> logger)
        {
            this.loginPage = loginPage;
            this.quickLinkPage = quickLinkPage;
            this.logger = logger;
            username = options.Value.BlogLogin;
            passowrd = options.Value.BlogPassword;
        }
        

        public List<Episode> ShortenAllLinks(List<Episode> episodes)
        {
            loginPage.GoTo();
            loginPage.Login(username, passowrd);

            quickLinkPage.GoTo();

            foreach (var episode in episodes)
            {
                
                foreach (var article in episode.Articles)
                {
                    
                    quickLinkPage.GoTo();
                    var addLink = quickLinkPage.AddLink(article.Link);
                    article.Link = addLink;
                }
            }
            return episodes;
        }
    }
}