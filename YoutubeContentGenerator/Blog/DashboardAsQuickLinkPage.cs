using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using YoutubeContentGenerator.Settings;

namespace YoutubeContentGenerator.Blog
{
    public class DashboardAsQuickLinkPage :PageBase, IQuickLinkPage
    {
        public DashboardAsQuickLinkPage(IWebDriver driver, IOptions<WordPressOptions> options) : base(driver,options)
        {
        }

        public string AddLink(string url)
        {
            
            var link = Driver.FindElement(By.Id("prli-quick-create-slug")).GetAttribute("Value");
            
            Driver.FindElement(By.Id("prli-quick-create-url")).SendKeys(url);
            Driver.FindElement(By.CssSelector("#prli-quick-create input.button")).Click();


            //wait until main page load 
            Driver.FindElement(By.Id("the-list"));
            return $"{BaseUrl}/{link}";
            
        }
        

        public IQuickLinkPage GoTo()
        {
            Driver.FindElement(By.LinkText("Kokpit")).Click();
            return this;
        }
    }
}
