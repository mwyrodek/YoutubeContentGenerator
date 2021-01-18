using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using YCG.Models;
using YoutubeContentGenerator.Blog;

namespace YoutubeContentGenerator.SeleniumLinkShortener
{
    public class SeleniumShortenLinks : ILinkShortener
    {

        private readonly ILoginPage loginPage;
        private readonly IQuickLinkPage quickLinkPage;
        //todo move it to config
        private string username; 
        private string passowrd; 

        public SeleniumShortenLinks(ILoginPage loginPage, IQuickLinkPage quickLinkPage, IConfiguration configuration)
        {
            this.loginPage = loginPage;
            this.quickLinkPage = quickLinkPage;
            username = configuration["Authentication:BlogLogin"];
            passowrd = configuration["Authentication:BlogPassword"];
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