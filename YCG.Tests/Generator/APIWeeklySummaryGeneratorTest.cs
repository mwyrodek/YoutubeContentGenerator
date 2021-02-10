using System;
using System.Collections.Generic;
using NUnit.Framework;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using YCG.Models;
using YoutubeContentGenerator.Blog;
using YoutubeContentGenerator.WeeklySummaryGenerator;
using YoutubeContentGenerator.WeeklySummuryGenerator;
using YoutubeContentGenerator.WeeklySummuryGenerator.WordPressWrapper;

namespace YCG.Tests.Generator
{
    [TestFixture]
    public class APIWeeklySummaryGeneratorTest
    {
        private IFixture fixture;
        private ApiWeeklySummaryGenerator sut;
        

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
        }
        
        [Test]
        public void PostIsEmpty_NoOperationPerformed()
        {
            var wrapperMock = fixture.Freeze<Mock<IWordPressClientWrapper>>();

            sut = fixture.Create<ApiWeeklySummaryGenerator>();
            sut.Save();
            wrapperMock.Verify(w=>w.Post(It.IsAny<WeeklySummaryPost>(), It.IsAny<string>(), It.IsAny<DateTime>()),Times.Never);
        }
        
        [Test]
        public void Setup_ClientIsCreatedAndAuthenitcated()
        {
            //todo maybe this test should aslo mock options - revistit it in future
            var wrapperMock = fixture.Freeze<Mock<IWordPressClientWrapper>>();
            wrapperMock.Setup(a => a.CreateClient(It.IsAny<string>())).Returns(wrapperMock.Object);
            wrapperMock.Setup(a => a.Authenticate(It.IsAny<string>(),It.IsAny<string>())).Returns(wrapperMock.Object);
            sut = fixture.Create<ApiWeeklySummaryGenerator>();
            
            wrapperMock.Verify(w=>w.CreateClient(It.IsAny<string>()),Times.Once);
            wrapperMock.Verify(w=>w.Authenticate(It.IsAny<string>(), It.IsAny<string>()),Times.Once);
        }
        [Test]
        public void HappyPath_NoOperationPerformed()
        {
            var wrapperMock = fixture.Freeze<Mock<IWordPressClientWrapper>>();

            sut = fixture.Create<ApiWeeklySummaryGenerator>();
            sut.CreateWeeklySummaryDescription(fixture.Create<List<Episode>>());
            sut.Save();
            wrapperMock.Verify(w=>w.Post(It.IsAny<WeeklySummaryPost>(), It.IsAny<string>(), It.IsAny<DateTime>()),Times.Once);
        }

        // happy path
        //
    }
}