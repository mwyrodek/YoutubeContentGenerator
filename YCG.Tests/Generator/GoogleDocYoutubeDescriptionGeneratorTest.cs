using System.Collections.Generic;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;
using YCG.Models;
using YoutubeContentGenerator.EpisodeGenerator;
using YoutubeContentGenerator.EpisodeGenerator.GoogleAPI;

namespace YCG.Tests.Generator
{
    [TestFixture]
    public class GoogleDocYoutubeDescriptionGeneratorTest
    {
        private IFixture fixture;
        private GoogleDocYoutubeDescriptionGenerator sut;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [Test]
        public void HappyPth_Construct_AuthenticatesUserToService()
        {
            var googleDocApiMock = fixture.Freeze<Mock<IGoogleDocApi>>();
            sut = fixture.Create<GoogleDocYoutubeDescriptionGenerator>();
            googleDocApiMock.Verify(gda=>gda.Authenticate());
        }
        
        [Test]
        public void CreateEpisodesTest()
        {
            var episode = fixture.Create<List<Episode>>();
            var ydcMock = fixture.Freeze<Mock<IYoutubeDescriptionContent>>();

            sut = fixture.Create<GoogleDocYoutubeDescriptionGenerator>();
            sut.CreateEpisodesDescription(episode);
            
            ydcMock.Verify(yd=>yd.CreateEpisodesDescription(episode));
        }
        
        
        [Test]
        public void SaveDescriptionTest()
        {
            var episode = fixture.Create<List<Episode>>();
            var googleDocApiMock = fixture.Freeze<Mock<IGoogleDocApi>>();
            
            sut = fixture.Create<GoogleDocYoutubeDescriptionGenerator>();
            sut.CreateEpisodesDescription(episode);
            sut.Save();
            
            googleDocApiMock.Verify(gda=>gda.WriteFile(It.IsAny<string>()));
        }
    }
}