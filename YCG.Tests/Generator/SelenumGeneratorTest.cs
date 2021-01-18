using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;


using YCG.Models;
using YoutubeContentGenerator.Blog;
using YoutubeContentGenerator.SeleniumLinkShortener;

namespace YCG.Tests.Generator
{
    [TestFixture]
    public class SelenumGeneratorTest
    {
        private Mock<LoginPage> LoginMock;
        private Mock<QuickLinkPage> QLPMock;
        private SeleniumShortenLinks _sut; 

        [SetUp]
        public void Setup()
        {
            LoginMock = new Mock<LoginPage>();
            
            QLPMock = new Mock<QuickLinkPage>();
        }

        [Test]
        public void SeleniumGenerator_LoginActionPErformed()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());;

            var episodes = fixture.Create<List<Episode>>();

            var mockLoginPage = fixture.Freeze<Mock<ILoginPage>>();
            _sut = fixture.Create<SeleniumShortenLinks>();
            _sut.ShortenAllLinks(episodes);
            mockLoginPage.Verify(m=>m.Login(It.IsAny<string>(),It.IsAny<string>()));

        }
        
        [Test]
        public void SeleniumGenerator_LoginGoToActionPErformed()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());;

            var episodes = fixture.Create<List<Episode>>();

            var mockLoginPage = fixture.Freeze<Mock<ILoginPage>>();
            _sut = fixture.Create<SeleniumShortenLinks>();
            _sut.ShortenAllLinks(episodes);
            mockLoginPage.Verify(m=>m.GoTo());
        }
        
        
        [Test]
        public void SeleniumGenerator_OneLinkTransformed()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());;

            var test = "Test"; 
            var episodes = new List<Episode>()
            {
                new Episode()
                {
                    Articles = new List<Article>()
                    {
                        new Article(){Link = "testtestestet"}
                    }
                }
            };

            var mock = new Mock<IQuickLinkPage>();
            mock.Setup(s=>s.AddLink(It.IsAny<string>())).Returns(test);
            fixture.Inject(mock);
                
            _sut = fixture.Create<SeleniumShortenLinks>();
            var shortenAllLinks = _sut.ShortenAllLinks(episodes);
            mock.Verify(m=>m.GoTo());
            
            Assert.That(shortenAllLinks.First().Articles.First().Link,Is.EqualTo(test));

        }

        [Test]
        public void SeleniumGenerator_AllLinksTransfomered()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());;

            var test = "Test";
            var episodes = fixture.Create<List<Episode>>();

            var mock = new Mock<IQuickLinkPage>();
            mock.Setup(s=>s.AddLink(It.IsAny<string>())).Returns(test);
            fixture.Inject(mock);
                
            _sut = fixture.Create<SeleniumShortenLinks>();
            var shortenAllLinks = _sut.ShortenAllLinks(episodes);
            mock.Verify(m=>m.GoTo());
            var articles = shortenAllLinks.SelectMany(episodes => episodes.Articles);
            Console.WriteLine(articles.Count());
            Assert.That(articles.All(a => a.Link == test), Is.True);

        }
    }
}