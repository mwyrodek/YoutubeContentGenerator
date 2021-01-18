using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using YoutubeContentGenerator.Blog;

namespace YCG.Tests.Blog
{
    [TestFixture]
    public class QuickLinkPageTest
    {
        private Mock<IWebDriver> webDriverMock;

        [SetUp]
        public void Setup()
        {
            webDriverMock = new Mock<IWebDriver>();


        }
        
        [Test]
        public void GoTo_Calls_Navigate()
        {
            var calls = 0;
            webDriverMock.Setup(driver => driver.Navigate().GoToUrl(It.IsAny<string>())).Callback(() => calls++);

            var webDriver = webDriverMock.Object;
            var quickLinkPage = new QuickLinkPage(webDriver);
            quickLinkPage.GoTo();
            
            Assert.That(calls, Is.EqualTo(1));
        }

        
        [Test]
        public void Add_Link_ReturnsShortenLink()
        {
            string fakeLink = "fake";
            string shorturl = "kkkk";
            var sendkeyscount = 0;
            var callsClick = 0;
            var webElementOldURL= new Mock<IWebElement>();
            var webElementButtondURL= new Mock<IWebElement>();
            var webElementNewUrl = new Mock<IWebElement>();


            webElementOldURL.Setup(element => element.SendKeys(fakeLink)).Callback(()=>sendkeyscount++);
            webElementNewUrl.Setup(element => element.GetAttribute(It.IsAny<string>())).Returns(shorturl);
            webElementButtondURL.Setup(element => element.Click()).Callback(() => callsClick++);
            webDriverMock.SetupSequence(driver => driver.FindElement(It.IsAny<By>())).Returns(webElementOldURL.Object).Returns(webElementNewUrl.Object).Returns(webElementButtondURL.Object);

            var webDriver = webDriverMock.Object;
            var quickLinkPage = new QuickLinkPage(webDriver);
            var addLink = quickLinkPage.AddLink(fakeLink);

            Assert.That(addLink, Does.StartWith("https://wyrodek.pl/"));
            Assert.That(sendkeyscount, Is.EqualTo(1));
            Assert.That(callsClick, Is.EqualTo(1));
        }
    }
}