using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using YoutubeContentGenerator.Settings;

namespace YoutubeContentGenerator.Blog
{
    public class LoginPage :PageBase, ILoginPage
    {
        private ILogger logger;
        public LoginPage(IWebDriver driver, ILogger<LoginPage> logger, IOptions<WordPressOptions> options) : base(driver,options)
        {
            this.logger = logger;
        }


        public ILoginPage GoTo()
        {
            //wypadku zmiany url to i tak bedzie wymagało manaulej porpway przez zahardkodowany link w srodku
            //todo sprawdzic czy link dalej jest potrzebny
            var url = $"{BaseUrl}/wp-login.php?redirect_to=https%3A%2F%2Fwww.wyrodek.pl%2Fwp-admin%2Findex.php&auth=1";
            logger.LogTrace($"going to {url}");
            Driver.Navigate().GoToUrl(url);
            Thread.Sleep(500);
            return this;
        }

        public bool IsUserLogedIn()
        {
            var url = $"{BaseUrl}/wp-admin/index.php";
            Driver.Navigate().GoToUrl(url);
            try
            {
                Driver.FindElement(By.Id("title-wrap"));
            }
            catch
            {
                return false;
            }
            return true;
        }

        public ILoginPage Login(string user, string password)
        {
            if(!Driver.Url.Contains("wp-login"))
            {
                logger.LogInformation("User already logged int");
                return this;
            }
            logger.LogTrace($"Entering credentials for {user}");
            Driver.FindElement(By.Id("user_login")).Clear();
            Driver.FindElement(By.Id("user_login")).SendKeys(user);
            Driver.FindElement(By.Id("user_pass")).SendKeys(password);
            Driver.FindElement(By.Id("wp-submit")).Click();
            logger.LogTrace($"User Logged in");
            try
            {
                Driver.FindElement(By.Id("user_login")).Clear();
                Driver.FindElement(By.Id("user_login")).SendKeys(user);
                Driver.FindElement(By.Id("user_pass")).SendKeys(password);
                Driver.FindElement(By.Id("wp-submit")).Click();
            }
            catch (NoSuchElementException e)
            {
                logger.LogInformation("Anti automation not triggered");
            }
            return this;
        }

  
    }
}