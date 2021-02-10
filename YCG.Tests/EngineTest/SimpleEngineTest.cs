using System.Collections.Generic;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;
using YCG.Models;
using YCG.Tests.Pocket;
using YoutubeContentGenerator;
using YoutubeContentGenerator.Engine;
using YoutubeContentGenerator.EpisodeGenerator;
using YoutubeContentGenerator.LoadData;
using YoutubeContentGenerator.WeeklySummuryGenerator;

namespace YCG.Tests.EngineTest
{
    [TestFixture]
    public class SimpleEngineTest
    {
        private IFixture fixture;
        private SimpleEngine sut;
        [SetUp]
        public void Setup()
        {
            
            fixture = new Fixture().Customize(new AutoMoqCustomization());
        }
        [Test]
        public void LoadDataTest()
        {
            var testdata = fixture.Create<List<Episode>>();
            var mockLoadData = fixture.Freeze<Mock<ILoadData>>();
            mockLoadData.Setup(ld=>ld.Execute()).Returns(testdata);
            sut = fixture.Create<SimpleEngine>();
            sut.LoadData();
            Assert.That(sut.Episodes, Is.EqualTo(testdata));
            mockLoadData.Verify(d => d.Execute(), Times.Once);
        }

        [Test]
        public void GenerateLinksTest()
        {
            var mock = fixture.Freeze<Mock<ILinkShortener>>();
            sut = fixture.Create<SimpleEngine>();
            sut.GenerateLinks();

            mock.Verify(s => s.ShortenAllLinks(It.IsAny<List<Episode>>()), Times.Once);
        }

        [Test]
        public void LoadData_GenerateInteractionTest()
        {
            var testdata = fixture.Create<List<Episode>>();
            var mockLoadData = fixture.Freeze<Mock<ILoadData>>();
            mockLoadData.Setup(ld=>ld.Execute()).Returns(testdata);
            var mockShortener = fixture.Freeze<Mock<ILinkShortener>>();
            
            sut = fixture.Create<SimpleEngine>();
            sut.LoadData();
            sut.GenerateLinks();
            mockShortener.Verify(s => s.ShortenAllLinks(testdata), Times.Once);
        }
        
        [Test]
        public void GenerateDescription_UpdatesEpisodeNumber()
        {
            var testdata = fixture.Create<List<Episode>>();
            var mockLoadData = fixture.Freeze<Mock<ILoadData>>();
            mockLoadData.Setup(ld=>ld.Execute()).Returns(testdata);
            
            var mockEpisodeNumber = fixture.Freeze<Mock<IEpisodeNumberHelper>>();
            mockEpisodeNumber.Setup(en => en.GetLastEpisodeNumber()).Returns(1);
            sut = fixture.Create<SimpleEngine>();
            sut.LoadData();
            sut.GenerateDescription();
            mockEpisodeNumber.Verify(en=>en.UpdateLastEpisodeNumber(testdata.Count+1));
        }
        
        [Test]
        public void GenerateDescription_UpdatesEpisodeNumber_WrongValueRemovesUpdate()
        {
            var testdata = fixture.Create<List<Episode>>();
            var mockLoadData = fixture.Freeze<Mock<ILoadData>>();
            mockLoadData.Setup(ld=>ld.Execute()).Returns(testdata);
            
            var mockEpisodeNumber = fixture.Freeze<Mock<IEpisodeNumberHelper>>();
            mockEpisodeNumber.Setup(en => en.GetLastEpisodeNumber()).Returns(0);
            sut = fixture.Create<SimpleEngine>();
            sut.LoadData();
            sut.GenerateDescription();
            mockEpisodeNumber.Verify(en=>en.UpdateLastEpisodeNumber(It.IsAny<int>()), Times.Never);
        }
        
        //todo looks like GenerateDescription is doing to much...
        [Test]
        public void GenerateDescription_CreatesDescriptionAndSavesChanges()
        {
            var testdata = fixture.Create<List<Episode>>();
            var mockLoadData = fixture.Freeze<Mock<ILoadData>>();
            mockLoadData.Setup(ld=>ld.Execute()).Returns(testdata);
            
            var mockYoutubeDesc = fixture.Freeze<Mock<IYouTubeDescriptionGenerator>>();
            
            sut = fixture.Create<SimpleEngine>();
            sut.LoadData();
            sut.GenerateDescription();
            mockYoutubeDesc.Verify(yt=>yt.CreateEpisodesDescription(testdata), Times.Once);
            mockYoutubeDesc.Verify(yt=>yt.Save(), Times.Once);
        }
        
        [Test]
        public void GenerateWeekSummary_CreatesDescriptionAndSavesChanges()
        {
            var testdata = fixture.Create<List<Episode>>();
            var mockLoadData = fixture.Freeze<Mock<ILoadData>>();
            mockLoadData.Setup(ld=>ld.Execute()).Returns(testdata);
            
            var mockWeekDesc = fixture.Freeze<Mock<IWeeklySummaryGenerator>>();
            
            sut = fixture.Create<SimpleEngine>();
            sut.LoadData();
            sut.GenerateWeekSummary();
            
            mockWeekDesc.Verify(sg=>sg.CreateWeeklySummaryDescription(testdata), Times.Once);
            mockWeekDesc.Verify(sg=>sg.Save(), Times.Once);
        }
    }
}
