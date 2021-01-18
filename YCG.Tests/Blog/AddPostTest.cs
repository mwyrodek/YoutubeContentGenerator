using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using YoutubeContentGenerator.Blog;
using ILogger = Castle.Core.Logging.ILogger;

namespace YCG.Tests.Blog
{
    [TestFixture]
    public class AddPostTest
    {
        private ILogger<LoginPage> logger;
        private IConfiguration configuration;

        [Test]
        public void FullAddPost()
        {
            logger = Mock.Of<ILogger<LoginPage>>();
            
            var driver = new ChromeDriver();
            var login = new LoginPage(driver, logger,configuration);
            login.GoTo();
            login.Login("fake", "fake");

            var post = new AddPostPage(driver, configuration);
            
            var date = DateTime.UtcNow.AddYears(1);
            post.GoTo()
                .AddPostTittle("TestTittle")
                .AddPostBody("TestBody")
                .OpenSettingsMenu()
                .SetCategoty("ITea")
                .SchedulePost(date)
                ;

        }
    }
}