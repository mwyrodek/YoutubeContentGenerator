using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using YCG.Models;
using YoutubeContentGenerator.Blog;
using YoutubeContentGenerator.LoadData.Pocket;
using YoutubeContentGenerator.SeleniumLinkShortener;
using YoutubeContentGenerator.Settings;

namespace YCG.Tests.LinkShorteners
{
    [TestFixture]
    public class SeleniumLinkShortenerTest
    {
        

        //goes to quick page
        //translates link for each episode and article
        //negative paths
        
        private YoutubeContentGenerator.SeleniumLinkShortener.SeleniumLinkShortener sut;
        private Mock<IQuickLinkPage> mockQuickPage;
        private Mock<ILoginPage> mockLoginPage;
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            mockQuickPage = fixture.Freeze<Mock<IQuickLinkPage>>();
            //mockLoginPage = new Mock<ILoginPage>();
            mockLoginPage = fixture.Freeze<Mock<ILoginPage>>();
        }

        [Test]
        public void HappyPath_UserLogsIn()
        {
            var episode = fixture.Create<List<Episode>>();
            sut = fixture.Create<YoutubeContentGenerator.SeleniumLinkShortener.SeleniumLinkShortener>();
            sut.ShortenAllLinks(episode);
            mockLoginPage.Verify(lp=>lp.GoTo());
            mockLoginPage.Verify(lp=>lp.Login(It.IsAny<string>(),It.IsAny<string>()));
        }
        
        [Test]
        public void HappyPath_EpisodesAreChanged()
        {
            var episode = fixture.Create<List<Episode>>();
            sut = fixture.Create<YoutubeContentGenerator.SeleniumLinkShortener.SeleniumLinkShortener>();
            var actualEpisode =sut.ShortenAllLinks(episode);
            mockLoginPage.Verify(lp=>lp.GoTo());
            mockLoginPage.Verify(lp=>lp.Login(It.IsAny<string>(),It.IsAny<string>()));
            Assert.That(actualEpisode.Count, Is.EqualTo(episode.Count));
            //todo i need to add better comparer
            Assert.That(actualEpisode[0].Articles.Count, Is.EqualTo(episode[0].Articles.Count));
        }
        
        [Test]
        public void HappyPath_GoesToQuickPageAfterEveryCase()
        {
            var episode = fixture.Create<List<Episode>>();
            sut = fixture.Create<YoutubeContentGenerator.SeleniumLinkShortener.SeleniumLinkShortener>();
            sut.ShortenAllLinks(episode);
            
            var count = episode.SelectMany(d => d.Articles).Count();
            //todo i need to add better comparer
            mockQuickPage.Verify(qp=>qp.GoTo(),Times.Exactly(count+1));
            mockQuickPage.Verify(qp=>qp.AddLink(It.IsAny<string>()),Times.Exactly(count));
        }

    }
}