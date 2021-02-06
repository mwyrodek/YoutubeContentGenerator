using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using YoutubeContentGenerator.Blog;
using YoutubeContentGenerator.Settings;

namespace YCG.Tests.Blog
{
    public class LoginPageTest
    {
        private Mock<IWebDriver> webDriverMock;
        private ILogger<LoginPage> logger;
        private IOptions<WordPressOptions> options;
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            this.fixture = new Fixture().Customize(new AutoMoqCustomization());
            webDriverMock = fixture.Freeze<Mock<IWebDriver>>();
            //   logger.Setup(x => x.LogTrace(It.IsAny<string>()));

        }

        [Test]
        public void GoTo_Calls_Navigate()
        {




            var loginPage = fixture.Create<LoginPage>();
            loginPage.GoTo();
            
            webDriverMock.Verify(wd=>wd.Navigate().GoToUrl(It.IsAny<string>()));
        }
        
        //todo brokentest - needs fixing
        // [Test]
        // public void Login()
        // {
        //     string login = "log";
        //     string password = "pas";
        //     var callsLogin = 0;
        //     var callsPassword = 0;
        //     var callsClick = 0;
        //     var webElementLogin= new Mock<IWebElement>();
        //     var webElementPassword= new Mock<IWebElement>();
        //     var webElementButton = new Mock<IWebElement>();
        //
        //
        //     webElementLogin.Setup(element => element.SendKeys(login)).Callback(()=>callsLogin++);
        //     webElementPassword.Setup(element => element.SendKeys(password)).Callback(()=>callsPassword++);
        //     webElementButton.Setup(element => element.Click()).Callback(()=>callsClick++);
        //     webDriverMock.SetupSequence(driver => driver.FindElement(It.IsAny<By>())).Returns(webElementLogin.Object).Returns(webElementPassword.Object).Returns(webElementButton.Object);
        //     webDriverMock.SetupProperty(d => d.Manage().Timeouts().ImplicitWait, TimeSpan.Zero);
        //     //webDriverMock.SetupAllProperties();
        //
        //     var webDriver = webDriverMock.Object;
        //     var loginPage = new LoginPage(webDriver, logger, options);
        //     loginPage.Login(login,password);
        //     
        //     Assert.That(callsLogin, Is.EqualTo(1));
        //     Assert.That(callsPassword, Is.EqualTo(1));
        //     Assert.That(callsClick, Is.EqualTo(1));
        // }
        
        
        
        
    }
}