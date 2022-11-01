using System.Collections.Generic;
using NUnit.Framework;
using AutoFixture;
using AutoFixture.AutoMoq;
using NUnit.Framework.Internal.Execution;
using WordPressPCL.Models;
using YCG.Models;
using YoutubeContentGenerator.EpisodeGenerator;

namespace YCG.Tests.Generator
{
    [TestFixture]
    public class EpisodeBuilderTests
    {
        private IFixture fixture;
        
        [SetUp]
        public void Setup()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [Test]
        public void Build_ReturnsEpisode()
        {
            var testdata = fixture.Create<Episode>();
            var episodeBuilder = new EpisodeBuilder(testdata);

            var actualData = episodeBuilder.Build();
            
            Assert.AreSame(testdata,actualData);
        }
        
        [Test]
        public void AggregateTags_MovesTagsFromArticleToEpisode()
        {
            
            var a1 = new Article();
            a1.Tags = new List<string>() {"tag1", "tag2"};
            var a2 = new Article();
            a2.Tags = new List<string>() {"tag3", "tag4"};
            var episode = new Episode()
            {
                Articles = new List<Article>() {a1, a2}
            };
            
            var episodeBuilder = new EpisodeBuilder(episode);

            var actualData = episodeBuilder.AggregateTagsFromArticles().Build();
            Assert.That(actualData.Tags.Count, Is.EqualTo(4));
            Assert.That(actualData.Tags, Is.EquivalentTo(new List<string>() {"tag1", "tag2", "tag3", "tag4"}));
        }
        
        [Test]
        public void AggregateTags_ReaptingTagsWontBeRemoved()
        {
            
            var a1 = new Article();
            a1.Tags = new List<string>() {"tag1", "tag2"};
            var a2 = new Article();
            a2.Tags = new List<string>() {"tag1", "tag2"};
            var episode = new Episode()
            {
                Articles = new List<Article>() {a1, a2}
            };
            
            var episodeBuilder = new EpisodeBuilder(episode);

            var actualData = episodeBuilder.AggregateTagsFromArticles().Build();
            Assert.That(actualData.Tags.Count, Is.EqualTo(4));
            Assert.That(actualData.Tags, Is.EquivalentTo(new List<string>() {"tag1", "tag2", "tag1", "tag2"}));
        }
        
        [Test]
        public void RemoveRepeating_RemovesRepeatingTags()
        {
            var episode = new Episode();
            episode.Tags = new List<string>() {"tag1", "tag2", "tag1", "tag2"};
            var episodeBuilder = new EpisodeBuilder(episode);

            var actualData = episodeBuilder.RemoveRedundantTags().Build();
            Assert.That(actualData.Tags.Count, Is.EqualTo(2));
            Assert.That(actualData.Tags, Is.EquivalentTo(new List<string>() {"tag1", "tag2"}));
        }
        
        [Test]
        public void RemoveSpecialTags_RemovesSpecialTags()
        {
            var episode = new Episode();
            episode.Tags = new List<string>() {"tag1", "soft", "tag2", "Soft", "tag1", "tag2", "hard", "Hard"};
            var episodeBuilder = new EpisodeBuilder(episode);

            var actualData = episodeBuilder.RemoveSpecialTags().Build();
            Assert.That(actualData.Tags.Count, Is.EqualTo(4));
            Assert.That(actualData.Tags, Is.EquivalentTo(new List<string>() {"tag1", "tag2", "tag1", "tag2"}));
        }
        
        [Test]
        public void EmptyCOllection_SkipAddingTags()
        {
            var a1 = new Article();
            a1.Tags = null;

            var episode = new Episode()
            {
                Articles = new List<Article>() {a1}
            };
            var episodeBuilder = new EpisodeBuilder(episode);

            var actualData = episodeBuilder.AggregateTagsFromArticles().Build();
            Assert.That(actualData.Tags.Count, Is.EqualTo(1));
            Assert.That(actualData.Tags, Is.EquivalentTo(new List<string>() {"WARNING NO TAGS ADDED"}));
        }
    }
}