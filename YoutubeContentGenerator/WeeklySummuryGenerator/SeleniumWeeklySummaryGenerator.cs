using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using YCG.Models;
using YoutubeContentGenerator.Blog;

namespace YoutubeContentGenerator.WeeklySummuryGenerator
{
    public class SeleniumWeeklySummaryGenerator : IWeeklySummaryGenerator
    {
        private WeeklySummaryPost post;
        private readonly ILoginPage loginPage;
        private readonly IAddPostPage addPostPagePage;
        private ILogger logger;
        private string username;
        private string passowrd;

        public SeleniumWeeklySummaryGenerator(ILoginPage loginPage, IAddPostPage addPostPagePage, ILogger<SeleniumWeeklySummaryGenerator> logger, IConfiguration configuration)
        {
            this.loginPage = loginPage;
            this.addPostPagePage = addPostPagePage;
            this.logger = logger;
            username = configuration["Authentication:BlogLogin"];
            passowrd = configuration["Authentication:BlogPassword"];
        }

        public void CreateWeeklySummaryDescription(List<Episode> episodes)
        {
            post = WeeklySummaryContentGenerator.CreateWeeklySummaryContent(episodes);
            
        }

        public void Save()
        {
            var date = Dates.GetNextWeekSaturday();
            if(!loginPage.IsUserLogedIn())
            { 
                loginPage
                    .GoTo()
                    .Login(username,passowrd);
            }
            addPostPagePage.GoTo()
                .AddPostTittle(post.Title)
                .AddPostBody(post.Body)
                .OpenSettingsMenu()
                .SetCategoty("ITea")
                .SchedulePost(date)
                ;

        }
    }
}
