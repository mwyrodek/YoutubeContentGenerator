using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using YoutubeContentGenerator.Blog;
using YoutubeContentGenerator.Settings;

namespace YCG.Tests.Blog
{
    [TestFixture]
    public class DashboardAsQuickLinkPageTest
    {
        [TestFixture]
        public class ContentGeneratorTest
        {
            private IFixture fixture;
            

            [SetUp]
            public void Setup()
            {
                fixture = new Fixture().Customize(new AutoMoqCustomization());
            }

            //todo I doubt value of this test
            [Test]
            public void GoTo_Calls_Navigate()
            {
                var webDrvierMock = fixture.Freeze<Mock<IWebDriver>>();
                var quickLinkPage = fixture.Create<DashboardAsQuickLinkPage>();
                quickLinkPage.GoTo();
                webDrvierMock.Verify(d => d.FindElement(It.IsAny<By>()));
            }



            [Test]
            public void Add_Link_ReturnsShortenLink()
            {
                var fakeLink = "fake";
                var shorturl = "kkkk";
                const string url = "test.pl";

                var webDriverMock = fixture.Freeze<Mock<IWebDriver>>();
                var webElementMock = new Mock<IWebElement>();

                var wpOptions = new WordPressOptions();
                wpOptions.BlogUrl = url;
                
                var optionsMock = new Mock<IOptions<WordPressOptions>>();
                optionsMock.Setup(ap => ap.Value).Returns(wpOptions);
                fixture.Inject(optionsMock);
                
                webElementMock.Setup(element => element.GetAttribute(It.IsAny<string>())).Returns(shorturl);
                webDriverMock.Setup(driver => driver.FindElement(It.IsAny<By>()))
                    .Returns(webElementMock.Object);
                
                    
                var sut = fixture.Create<DashboardAsQuickLinkPage>();
                var addLink = sut.AddLink(fakeLink);

                Assert.That(addLink, Does.StartWith(url));
            }
        }
    }
}