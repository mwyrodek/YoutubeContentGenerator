using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using NUnit.Framework;
using PocketSharp.Models;
using YoutubeContentGenerator.LoadData.Pocket;

namespace YCG.Tests.Pocket
{
    /// <summary>
    /// note: At this point I am not sure if there is even a value in testing mapping
    /// </summary>
    [TestFixture]
    public class PocketMapperTest
    {
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
        }
        
        [Test]
        public void PocketItemToArticle()
        {
            var pocketItem = fixture.Create<PocketItem>();

            var article = PocketMapper.Map(pocketItem);
            
            Assert.That(article.Link, Is.EqualTo(pocketItem.Uri.ToString()));
            Assert.That(article.Title, Is.EqualTo(pocketItem.Title));
            Assert.That(article.Tags[0], Is.EqualTo(pocketItem.Tags.First().Name));
        }
        
        
        [Test]
        public void PocketArticleToArticle()
        {
            var pocketArticle = fixture.Create<PocketArticle>();

            var article = PocketMapper.Map(pocketArticle);
            
            Assert.That(article.Link, Is.EqualTo(pocketArticle.Uri.ToString()));
            Assert.That(article.Title, Is.EqualTo(pocketArticle.Title));
        }
    }
}