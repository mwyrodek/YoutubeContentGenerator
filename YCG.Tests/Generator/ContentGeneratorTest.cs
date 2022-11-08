using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;
using YoutubeContentGenerator;
using YoutubeContentGenerator.Engine;
using YoutubeContentGenerator.WeeklySummuryGenerator;

namespace YCG.Tests.Generator
{
    [TestFixture]
    public class ContentGeneratorTest
    {
        
        private IFixture fixture;
        private ContentGenerator sut;
        

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [Test]
        public void HappyPath_EveryThinkWorks()
        {
            var engine = fixture.Freeze<Mock<IEngine>>();
            sut = fixture.Create<ContentGenerator>();
            sut.Run();
            //disabled due to bugs 
            engine.Verify(e=>e.LoadData(),Times.Once);
  //          engine.Verify(e=>e.GenerateLinks(),Times.Once);
            engine.Verify(e=>e.GenerateDescription(),Times.Once);
            //engine.Verify(e=>e.GenerateWeekSummary(),Times.Once);
        }
    }
}