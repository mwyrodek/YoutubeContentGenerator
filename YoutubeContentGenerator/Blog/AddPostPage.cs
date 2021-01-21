using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OpenQA.Selenium.Support.UI;
using TextCopy;
using YoutubeContentGenerator.Settings;

namespace YoutubeContentGenerator.Blog
{
    public class AddPostPage : PageBase, IAddPostPage
    {
        public AddPostPage(IWebDriver Driver,  IOptions<WordPressOptions> options) : base(Driver,options)
        {
        }

        public IAddPostPage AddPostBody(string body)
        {
            ClipboardService.SetText("body");
            string block = "div.block-editor-block-list__layout.is-root-container div[data-root-client-id]";
            
            new Actions(Driver).Click(Driver.FindElement(By.CssSelector("div.block-editor-block-list__layout.is-root-container textarea"))).Perform();
            new Actions(Driver).KeyDown(Keys.Control)
                .SendKeys(Driver.FindElement(By.CssSelector("div.block-editor-block-list__layout.is-root-container p")), "v")
                .KeyUp(Keys.Control)
                .Perform();

            return this;
        }

        public IAddPostPage AddPostTittle(string title)
        {
            Driver.FindElement(By.Id("post-title-0")).SendKeys(title);
            return this;
        }

        public IAddPostPage GoTo()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/wp-admin/post-new.php");
            try
            {
                Driver.FindElement(By.CssSelector("button[aria-label='Close dialogue']")).Click();
            }
            catch(NoSuchElementException)
            {
                //todo add logger
            }
            return this;
        }

        public IAddPostPage OpenSettingsMenu()
        {
            //shortcut ctrl+shift+,
            try
            {
                Driver.FindElement(By.CssSelector("button[aria-label~='Post']")).Click();
            }
            catch (NoSuchElementException)
            {
                Actions actions = new Actions(Driver);
                actions
                    .KeyDown(Keys.Control)
                    .KeyDown(Keys.Shift)
                    .SendKeys(",")
                    .KeyUp(Keys.Shift)
                    .KeyUp(Keys.Control)
                    .Perform();
                Driver.FindElement(By.CssSelector("button[aria-label~='Post']")).Click();
            }

            return this;

        }

        public IAddPostPage SchedulePost(DateTime date)
        {
            //nacinsj na napis "natychmiast"
            Thread.Sleep(1000); //teraz juz mam dośc
            Driver.FindElement(By.CssSelector("div.components-panel__row.edit-post-post-schedule button")).Click();
            
            //wybierz date z date pikera
            //day
            //var day= Driver.FindElement(By.CssSelector("input[aria-label='Day']"));
            Driver.FindElement(By.CssSelector("input[aria-label='Day']")).SendKeys($"{Keys.Backspace}{Keys.Backspace}{Keys.Backspace}{Keys.Backspace}{date.Day.ToString()}");
            //year;
             Driver.FindElement(By.CssSelector("input[aria-label='Year']")).SendKeys($"{Keys.Backspace}{Keys.Backspace}{Keys.Backspace}{Keys.Backspace}{date.Year.ToString()}");
            
            //month
            
            var element = Driver.FindElement(By.CssSelector("select[aria-label='Month']"));
            SelectElement selectElement = new SelectElement(element);
            DateTimeFormatInfo dtfi = CultureInfo.GetCultureInfo("en-Us").DateTimeFormat;
            var monthName = dtfi.GetMonthName(date.Month);
            selectElement.SelectByText(monthName);

            //publish
            Driver.FindElement(By.CssSelector("button.editor-post-publish-button__button")).Click();
            Driver.FindElement(By.CssSelector(".editor-post-publish-panel__header-publish-button button.editor-post-publish-button__button")).Click();

            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            wait.Until( ExpectedConditions.ElementExists(By.CssSelector("div.post-publish-panel__postpublish")));

            return this;
        }


        public IAddPostPage SetCategoty(string category)
        {
            //i hate wordpress locators
            Driver.FindElement(By.XPath("//*[@id='editor']/div/div/div[1]/div/div[1]/div[2]/div[2]/div/div[3]/div[4]/h2/button")).Click();           
            Driver.FindElement(By.CssSelector(".edit-post-sidebar input[type='search']")).SendKeys(category);
            Driver.FindElement(By.CssSelector(".editor-post-taxonomies__hierarchical-terms-list input.components-checkbox-control__input")).Click();

            return this;
        }
    }
}
