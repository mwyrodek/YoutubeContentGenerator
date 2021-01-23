using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using YoutubeContentGenerator.LoadData.Pocket;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using YCG.Models;
using YoutubeContentGenerator.Settings;

namespace YCG.Tests.Pocket
{
    [TestFixture]
    public class LoadDataFromPocketTests
    {
        private LoadDataFromPocket sut;
        private Mock<ILogger<LoadDataFromPocket>> loggerMock;
        private Mock<IOptions<PocketOptions>> optionsMock;
        private Mock<IPocketConector> pocketConector;

        [SetUp]
        public void Setup()
        {
            loggerMock = new Mock<ILogger<LoadDataFromPocket>>();

            optionsMock = new Mock<IOptions<PocketOptions>>();
            pocketConector = new Mock<IPocketConector>();
        }

        [TestCase(1)]
        [TestCase(5)]
        public void SeasonLengthIsRespected(int seasonLength)
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            //Arrange
            var pocketOptions = fixture.Create<PocketOptions>();
            pocketOptions.SeasonLength = seasonLength;

            optionsMock.Setup(o => o.Value).Returns(pocketOptions);
            fixture.Inject(optionsMock);
            this.sut = fixture.Create <LoadDataFromPocket>();
            //Act
            var result = sut.Execute();


            Assert.That(result.Count, Is.EqualTo(seasonLength));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void SeasonHasToBeLongerThenZero(int seasonLength)
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            //Arrange

            var pocketOptions = fixture.Create<PocketOptions>();
            pocketOptions.SeasonLength = seasonLength;

            optionsMock.Setup(o => o.Value).Returns(pocketOptions);
            this.sut = new LoadDataFromPocket(loggerMock.Object, optionsMock.Object, pocketConector.Object);
            //Act
            Assert.Throws<ArgumentOutOfRangeException>(
                () => { sut.Execute(); }
            );
        }
        

        [Test]
        public void NoTagsEndsInError()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            //Arrange

            var pocketOptions = fixture.Create<PocketOptions>();
            pocketOptions.SeasonLength = 2;
            pocketOptions.Tags = new List<string>();

            optionsMock.Setup(o => o.Value).Returns(pocketOptions);
            this.sut = new LoadDataFromPocket(loggerMock.Object, optionsMock.Object, pocketConector.Object);
            //Act
            Assert.Throws<ArgumentOutOfRangeException>(
                () => { sut.Execute(); }
            );
        }

        [Test]
        public void HappyPath_EachEpisodeHasAllArticle()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            //Arrange

            var pocketOptions = fixture.Create<PocketOptions>();
            pocketOptions.SeasonLength = 2;
            pocketOptions.Tags = new List<string> {"Test", "Test2"};
            optionsMock.Setup(o => o.Value).Returns(pocketOptions);
            fixture.Inject(optionsMock);
            this.sut = fixture.Create<LoadDataFromPocket>();
            //Act

            var result = sut.Execute();
            
            Assert.That(result.Any(e=>e.Articles.Count == pocketOptions.Tags.Count), Is.True,"Not All Episodes had all articles");
        }
        
        [Test]
        public void EdgeCase_OnaTagHadToFewAllArticle()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            //Arrange

            var pocketOptions = fixture.Create<PocketOptions>();
            pocketOptions.SeasonLength = 4;
            const string tagWithArticles = "Test";
            const string tagWithoutArticles = "Test2";
            pocketOptions.Tags = new List<string> {tagWithArticles, tagWithoutArticles};

            optionsMock.Setup(o => o.Value).Returns(pocketOptions);
            pocketConector.Setup(pc => pc.MoveArticleFromPocketByTag(tagWithArticles))
                .Returns(fixture.Create<Article>());

            pocketConector.SetupSequence(pc => pc.MoveArticleFromPocketByTag(tagWithoutArticles))
                .Returns(fixture.Create <Article>())
                .Returns((Article) null)
                .Returns((Article) null)
                .Returns((Article) null);
            fixture.Inject(optionsMock);
            fixture.Inject(pocketConector);
            loggerMock = fixture.Freeze<Mock<ILogger<LoadDataFromPocket>>>();
            
            //this.sut = new LoadDataFromPocket(loggerMock.Object, optionsMock.Object, pocketConector.Object);
            sut = fixture.Create<LoadDataFromPocket>(); 
            //Act

            var result = sut.Execute();
            
            Assert.That(result.FirstOrDefault().Articles.Count, Is.EqualTo(2),"First Epsiode  should be full length");
            Assert.That(result.Count(e => e.Articles.Count < 2), Is.EqualTo(3),"not all episde had expected artcile count");
        }
        
        
        [Test]
        public void EdgeCase_AllTagsHadToFewArticlesSeasonIsShorter()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            //Arrange

            var pocketOptions = fixture.Create<PocketOptions>();
            pocketOptions.SeasonLength = 4;
            const string tagWithArticles = "Test";
            const string tagWithoutArticles = "Test2";
            pocketOptions.Tags = new List<string> {tagWithArticles, tagWithoutArticles};

            optionsMock.Setup(o => o.Value).Returns(pocketOptions);
            pocketConector.SetupSequence(pc => pc.MoveArticleFromPocketByTag(tagWithArticles))
                .Returns(fixture.Create <Article>())
                .Returns(fixture.Create <Article>())
                .Returns((Article) null)
                .Returns((Article) null);
            pocketConector.SetupSequence(pc => pc.MoveArticleFromPocketByTag(tagWithoutArticles))
                .Returns(fixture.Create <Article>())
                .Returns((Article) null)
                .Returns((Article) null)
                .Returns((Article) null);
            fixture.Inject(optionsMock);
            fixture.Inject(pocketConector);
            loggerMock = fixture.Freeze<Mock<ILogger<LoadDataFromPocket>>>();
            
            //this.sut = new LoadDataFromPocket(loggerMock.Object, optionsMock.Object, pocketConector.Object);
            sut = fixture.Create<LoadDataFromPocket>(); 
            //Act

            var result = sut.Execute();
            
            Assert.That(result.Count, Is.EqualTo(2),"Season expected to be shorter due to lack of articles!");
            Assert.That(result[0].Articles.Count, Is.EqualTo(2),"First Epside should be full");
            Assert.That(result[1].Articles.Count, Is.EqualTo(1),"Second episode should have 1 article");
            
        }
    }
}