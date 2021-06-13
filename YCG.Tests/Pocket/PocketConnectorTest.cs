using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;
using PocketSharp;
using PocketSharp.Models;
using YoutubeContentGenerator.LoadData.Pocket;
using YoutubeContentGenerator.Settings;

namespace YCG.Tests.Pocket
{
    [TestFixture]
    public class PocketConnectorTest
    {
        private IFixture fixture;
        private PocketConector sut;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [Test]
        public void HappyPath_Returns_NewestUnreadArticleForTag()
        {
            string testtag = "Tag";
            var items = fixture.Create<IEnumerable<PocketItem>>();

            var mockPocketClient = new Mock<IPocketClient>();
            mockPocketClient
                .Setup(pc => pc.Get(It.IsAny<State?>(), It.IsAny<bool?>(), It.IsAny<string>(), It.IsAny<ContentType?>(),
                    It.IsAny<Sort?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<int?>(),
                    It.IsAny<int?>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(items);

            var mockPocketFactory = new Mock<IPocketFactory>();
            mockPocketFactory.Setup(pf => pf.CreatePocketClient()).Returns(mockPocketClient.Object);
            fixture.Inject(mockPocketFactory.Object);
            fixture.Inject(mockPocketClient.Object);

            sut = fixture.Create<PocketConector>();
            var article = sut.MoveArticleFromPocketByTag(testtag);

            mockPocketClient.Verify(v =>
                v.Get(State.unread, null, testtag, null, Sort.newest, null, null, null, null, null, default));
        }
        //todo

        [Test]
        public void EdgaCase_TagHasNoArticles_Returns_NewestUnreadArticleForTag()
        {
            string testtag = "Tag";
            var items = new List<PocketItem>();
            
            var mockPocketClient = new Mock<IPocketClient>();
            mockPocketClient
                .Setup(pc => pc.Get(It.IsAny<State?>(), It.IsAny<bool?>(), It.IsAny<string>(), It.IsAny<ContentType?>(),
                    It.IsAny<Sort?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<int?>(),
                    It.IsAny<int?>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(items);

            var mockPocketFactory = new Mock<IPocketFactory>();
            mockPocketFactory.Setup(pf => pf.CreatePocketClient()).Returns(mockPocketClient.Object);
            fixture.Inject(mockPocketFactory.Object);
            fixture.Inject(mockPocketClient.Object);

            sut = fixture.Create<PocketConector>();
            var article = sut.MoveArticleFromPocketByTag(testtag);
            Assert.IsNull(article);
            mockPocketClient.Verify(v =>
                v.Get(State.unread, null, testtag, null, Sort.newest, null, null, null, null, null, default));
        }

        //tag that doesnt exist - warning skipped
        //returns article
        //deletes article from pocket - only not test
        
        [Test]
        public void HappyPathDelteArticle_TagHasNoArticles_Returns_NewestUnreadArticleForTag()
        {
            string testtag = "Tag";
            var item = fixture.Create<PocketItem>();
            var items = new List<PocketItem>
            {
                item
            };

            var mockPocketClient = new Mock<IPocketClient>();
            mockPocketClient
                .Setup(pc => pc.Get(It.IsAny<State?>(), It.IsAny<bool?>(), It.IsAny<string>(), It.IsAny<ContentType?>(),
                    It.IsAny<Sort?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<int?>(),
                    It.IsAny<int?>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(items);

            var mockPocketFactory = new Mock<IPocketFactory>();
            mockPocketFactory.Setup(pf => pf.CreatePocketClient()).Returns(mockPocketClient.Object);
            fixture.Inject(mockPocketFactory.Object);
            fixture.Inject(mockPocketClient.Object);

            sut = fixture.Create<PocketConector>();
            sut.MoveArticleFromPocketByTag(testtag);
            
            mockPocketClient.Verify(pc=>pc.Archive(item.ID, It.IsAny<CancellationToken>()));
        }
        
        [Test]
        public void HappyPathArticleIsMaped_TagHasNoArticles_Returns_NewestUnreadArticleForTag()
        {
            string testtag = "Tag";
            var item = fixture.Create<PocketItem>();
            var items = new List<PocketItem>
            {
                item
            };
            var mappedArt = PocketMapper.Map(item);
            var mockPocketClient = new Mock<IPocketClient>();
            mockPocketClient
                .Setup(pc => pc.Get(It.IsAny<State?>(), It.IsAny<bool?>(), It.IsAny<string>(), It.IsAny<ContentType?>(),
                    It.IsAny<Sort?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<int?>(),
                    It.IsAny<int?>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(items);

            var mockPocketFactory = new Mock<IPocketFactory>();
            mockPocketFactory.Setup(pf => pf.CreatePocketClient()).Returns(mockPocketClient.Object);
            fixture.Inject(mockPocketFactory.Object);
            fixture.Inject(mockPocketClient.Object);

            sut = fixture.Create<PocketConector>();
           var result =  sut.MoveArticleFromPocketByTag(testtag);
           Assert.AreEqual(mappedArt.Title, result.Title);
           Assert.AreEqual(mappedArt.Link, result.Link);
            
            
        }
        //doesn't delete article - only on Test
    }
}