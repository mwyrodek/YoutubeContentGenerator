using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using YoutubeContentGenerator.Settings;

namespace YoutubeContentGenerator.Blog
{
    public abstract class PageBase
    {
        protected IWebDriver Driver;
        protected string BaseUrl;
        public PageBase(IWebDriver driver, IOptions<WordPressOptions> options)
        {
            this.Driver = driver;
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            BaseUrl = options.Value.BlogUrl;

        }
    }
}