using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using YoutubeContentGenerator.Blog;
using YoutubeContentGenerator.Settings;

namespace YCG.Tests.Blog
{
    public class LoginPageTest
    {
        private Mock<IWebDriver> webDriverMock;

        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            this.fixture = new Fixture().Customize(new AutoMoqCustomization());
            webDriverMock = fixture.Freeze<Mock<IWebDriver>>();
        }

        [Test]
        public void GoTo_Calls_Navigate()
        {
            var loginPage = fixture.Create<LoginPage>();
            loginPage.GoTo();
            
            webDriverMock.Verify(wd=>wd.Navigate().GoToUrl(It.IsAny<string>()));
        }
        
        //todo brokentest - needs fixing
        [Test]
        public void HappyPath_Login()
        {
            string login = "log";
            string password = "pas";
            var callsPassword = 0;
            var callsClick = 0;
            var anyWebElement= new Mock<IWebElement>();
            var loggerMock = new Mock<ILogger<LoginPage>>();
        
            var wpOptions = new WordPressOptions();
            wpOptions.BlogUrl = "testurl.com";
                
            var optionsMock = new Mock<IOptions<WordPressOptions>>();
            optionsMock.Setup(ap => ap.Value).Returns(wpOptions);
            webDriverMock.Setup(wd => wd.Url).Returns("wp-login");
            anyWebElement.SetupSequence(element => element.SendKeys(login)).Pass().Throws<NoSuchElementException>();
            anyWebElement.Setup(element => element.SendKeys(password)).Callback(()=>callsPassword++);
            anyWebElement.Setup(element => element.Click()).Callback(()=>callsClick++);
            webDriverMock.Setup(driver => driver.FindElement(It.IsAny<By>())).Returns(anyWebElement.Object);
            webDriverMock.SetupProperty(d => d.Manage().Timeouts().ImplicitWait, TimeSpan.Zero);
            
        
            var webDriver = webDriverMock.Object;
            var loginPage = new LoginPage(webDriver, loggerMock.Object, optionsMock.Object);
            loginPage.Login(login,password);
            
            Assert.That(callsPassword, Is.EqualTo(1));
            Assert.That(callsClick, Is.EqualTo(1));
        }
        
        [Test]
        public void HappyPath_NeededRe_Login()
        {
            const string login = "log";
            const string password = "pas";
            var callsLogin = 0;
            var callsPassword = 0;
            var callsClick = 0;
            var anyWebElement= new Mock<IWebElement>();
            var loggerMock = new Mock<ILogger<LoginPage>>();
        
            var wpOptions = new WordPressOptions();
            wpOptions.BlogUrl = "testurl.com";
                
            var optionsMock = new Mock<IOptions<WordPressOptions>>();
            optionsMock.Setup(ap => ap.Value).Returns(wpOptions);
            webDriverMock.Setup(wd => wd.Url).Returns("wp-login");
            anyWebElement.Setup(element => element.SendKeys(login)).Callback(()=>callsLogin++);
            anyWebElement.Setup(element => element.SendKeys(password)).Callback(()=>callsPassword++);
            anyWebElement.Setup(element => element.Click()).Callback(()=>callsClick++);
            webDriverMock.Setup(driver => driver.FindElement(It.IsAny<By>())).Returns(anyWebElement.Object);
            webDriverMock.SetupProperty(d => d.Manage().Timeouts().ImplicitWait, TimeSpan.Zero);
            
        
            var webDriver = webDriverMock.Object;
            var loginPage = new LoginPage(webDriver, loggerMock.Object, optionsMock.Object);
            loginPage.Login(login,password);
            
            Assert.That(callsLogin, Is.EqualTo(2));
            Assert.That(callsPassword, Is.EqualTo(2));
            Assert.That(callsClick, Is.EqualTo(2));
        }
        
                
        [Test]
        public void HappyPath_AlreadyLogged()
        {
            const string login = "log";
            const string password = "pas";
            var callsLogin = 0;
            var callsPassword = 0;
            var callsClick = 0;
            var anyWebElement= new Mock<IWebElement>();
            var loggerMock = new Mock<ILogger<LoginPage>>();
        
            var wpOptions = new WordPressOptions();
            wpOptions.BlogUrl = "testurl.com";
                
            var optionsMock = new Mock<IOptions<WordPressOptions>>();
            optionsMock.Setup(ap => ap.Value).Returns(wpOptions);
            webDriverMock.Setup(wd => wd.Url).Returns(String.Empty);
            anyWebElement.Setup(element => element.SendKeys(login)).Callback(()=>callsLogin++);
            anyWebElement.Setup(element => element.SendKeys(password)).Callback(()=>callsPassword++);
            anyWebElement.Setup(element => element.Click()).Callback(()=>callsClick++);
            webDriverMock.Setup(driver => driver.FindElement(It.IsAny<By>())).Returns(anyWebElement.Object);
            webDriverMock.SetupProperty(d => d.Manage().Timeouts().ImplicitWait, TimeSpan.Zero);
            
        
            var webDriver = webDriverMock.Object;
            var loginPage = new LoginPage(webDriver, loggerMock.Object, optionsMock.Object);
            loginPage.Login(login,password);
            
            Assert.That(callsLogin, Is.EqualTo(0));
            Assert.That(callsPassword, Is.EqualTo(0));
            Assert.That(callsClick, Is.EqualTo(0));
        }
    }
}