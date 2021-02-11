using System.Collections.Generic;
using NUnit.Framework;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using YCG.Models;
using YoutubeContentGenerator.EpisodeGenerator;

namespace YCG.Tests.Generator
{
    [TestFixture]
    public class YoutubeDescriptionGeneratorTextTest
    {
        private IFixture fixture;
        private YouTubeDescriptionGeneratorText sut;
        [SetUp]
        public void Setup()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [Test]
        public void CreateEpisodesTest()
        {
            var episode = fixture.Create<List<Episode>>();
            var ydcMock = fixture.Freeze<Mock<IYoutubeDescriptionContent>>();

            sut = fixture.Create<YouTubeDescriptionGeneratorText>();
            sut.CreateEpisodesDescription(episode);
            
            ydcMock.Verify(yd=>yd.CreateEpisodesDescription(episode));
        }
    }
}