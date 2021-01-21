using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace YoutubeContentGenerator.Blog
{
    [Obsolete]
    public class QuickLinkPage : IQuickLinkPage 
    {
        private IWebDriver driver;
      

        public QuickLinkPage(IWebDriver driver)
        {
            this.driver = driver;
            this.driver.Manage().Timeouts().ImplicitWait=TimeSpan.FromSeconds(15);
        }

        public IQuickLinkPage GoTo()
        {
            driver.Navigate().GoToUrl("https://wyrodek.pl/wp-admin/post-new.php?post_type=pretty-link");
            return this;
        }

        public string AddLink(string url)
        {
            driver.FindElement(By.Id("prli_url")).SendKeys(url);
            var attribute = driver.FindElement(By.Id("prli_slug")).GetAttribute("value");

            //try
            //{
            //    driver.FindElement(By.CssSelector("#prli-rating-popup .prli-rating-enjoy-no-popup")).Click();
            //}
            //catch(EntryPointNotFoundException)
            //{
            //    //pop didn't show up - that is goodl
            //}
            
            driver.FindElement(By.Id("publish")).Click();
            
            
            //wait until main page load 
            driver.FindElement(By.Id("the-list"));
            return$"https://wyrodek.pl/{attribute}";
        }

        public void DealWithPopUP()
        {
            try
            {
                var popup= driver.FindElement(By.Id("prli-upgrade-popup"));
                popup.FindElement(By.CssSelector(".prli-delay-popup")).Click();
            }
            catch(NoSuchElementException)
            {
                //skip
            }
        }
    }


}