using System;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;

namespace YoutubeContentGenerator.Blog
{
    public abstract class PageBase
    {
        protected IWebDriver Driver;
        protected string BaseUrl;
        public PageBase(IWebDriver driver, IConfiguration configuration)
        {
            this.Driver = driver;
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            BaseUrl = configuration["Authentication:BlogUrl"];

        }
    }
}