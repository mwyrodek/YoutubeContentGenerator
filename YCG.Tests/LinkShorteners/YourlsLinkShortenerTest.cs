using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;
using YCG.Models;
using YoutubeContentGenerator.LinkShortener;

namespace YCG.Tests.LinkShorteners
{
    [TestFixture]
    public class YourlsLinkShortenerTest
    {
        private YourlsLinkShortener sut;
        private Mock<IYourlsApi> yourlsApiMock;
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            yourlsApiMock = fixture.Freeze<Mock<IYourlsApi>>();
        }
        
        [Test]
        public void HappyPath_EpisodesAreChanged()
        {
            var expectedLink = "fakeurl";
            yourlsApiMock.Setup(a => a.ShortenUrl(It.IsAny<string>())).Returns(expectedLink);
            var episode = fixture.Create<List<Episode>>();
            sut = fixture.Create<YourlsLinkShortener>();
            var actualEpisode =sut.ShortenAllLinks(episode);

            Assert.That(actualEpisode.Count, Is.EqualTo(episode.Count));
            //todo i need to add better comparer
            Assert.That(actualEpisode[0].Articles.Count, Is.EqualTo(episode[0].Articles.Count));
            Assert.That(actualEpisode.SelectMany(ae=>ae.Articles).All(a=>a.Link==expectedLink));
        }
        
        [Test]
        public void EdgeCase_ApiErrorDoesntChangeUrl()
        {
            var expectedLink = "fakeurl";
            yourlsApiMock.Setup(a => a.ShortenUrl(It.IsAny<string>())).Throws<Exception>();
            var episode = fixture.Create<List<Episode>>();
            sut = fixture.Create<YourlsLinkShortener>();
            var actualEpisode =sut.ShortenAllLinks(episode);

            Assert.That(actualEpisode.Count, Is.EqualTo(episode.Count));
            
            Assert.That(actualEpisode[0].Articles.Count, Is.EqualTo(episode[0].Articles.Count));
            Assert.That(actualEpisode.SelectMany(ae=>ae.Articles).All(a=>a.Link!=expectedLink));
        }
        
        [Test]
        public void EdgeCase_ApiAuthorizationError_SkipsChaingOtherUrls()
        {
            var expectedLink = "fakeurl";
            yourlsApiMock.Setup(a => a.ShortenUrl(It.IsAny<string>())).Throws<UnauthorizedAccessException>();
            var episode = fixture.Create<List<Episode>>();
            sut = fixture.Create<YourlsLinkShortener>();
            var actualEpisode =sut.ShortenAllLinks(episode);

            Assert.That(actualEpisode.Count, Is.EqualTo(episode.Count));
            
            Assert.That(actualEpisode[0].Articles.Count, Is.EqualTo(episode[0].Articles.Count));
            Assert.That(actualEpisode.SelectMany(ae=>ae.Articles).All(a=>a.Link!=expectedLink));
            yourlsApiMock.Verify(ya=>ya.ShortenUrl(It.IsAny<string>()),Times.Once);
        }
    }
}