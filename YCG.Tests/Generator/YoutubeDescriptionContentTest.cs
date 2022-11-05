using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework.Constraints;
using WordPressPCL.Models;
using YCG.Models;
using YoutubeContentGenerator.EpisodeGenerator;
using YoutubeContentGenerator.EpisodeGenerator.GoogleAPI;

namespace YCG.Tests.Generator
{
    [TestFixture]
    public class YoutubeDescriptionContentTest
    {
        private IFixture fixture;
        private YoutubeDescriptionContent sut;
        
        [SetUp]
        public void Setup()
        {
            
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            sut = new YoutubeDescriptionContent();
        }

        //empty episode list

        [Test]
        public void HappyPath_CreateEpisode()
        {
            var episode = fixture.Create<Episode>();
            var result = sut.CreateEpisodeDescription(episode);
            
            Assert.Multiple(() =>
            {
                 StringAssert.StartsWith("<TITTLE> 🍵 📰 ITea Morning", result);
                 StringAssert.Contains($"{episode.EpisodeNumber}", result);
                 StringAssert.Contains($"<DESCRIPTION>", result);
                 episode.Articles.ForEach(a => StringAssert.Contains(a.Link, result));
                 episode.Articles.ForEach(a => StringAssert.Contains(a.Title, result));
            });
        }
        [Test]
        public void HappyPath_CreateEpisode_HasTittleWithEpisodeNumber()
        {
            var episode = fixture.Create<Episode>();
            var result = sut.CreateEpisodeDescriptionFormated(episode);
            
            Assert.Multiple(() =>
            {
                Assert.That(result.First().Content, Contains.Substring("<TITTLE> 🍵 📰 ITea Morning "));
                Assert.That(result.First().Content, Contains.Substring($"{episode.EpisodeNumber}"));
                Assert.That(result.First().ContentStyle, Is.EqualTo(ContentStyle.HEADING_2));
            });
        }
        [Test]
        public void HappyPath_CreateEpisodeFormated_HasBody()
        {
            var episode = fixture.Create<Episode>();
            var result = sut.CreateEpisodeDescriptionFormated(episode);
            
            Assert.Multiple(() =>
            {
                Assert.That(result[1].Content, Contains.Substring("<DESCRIPTION>"));
                episode.Articles.ForEach(a => StringAssert.Contains(a.Link, result[1].Content));
                episode.Articles.ForEach(a => StringAssert.Contains(a.Title, result[1].Content));
                Assert.That(result[1].Content, Contains.Substring("Timestamps"));
                episode.Tags.ForEach(a => StringAssert.Contains(a, result[1].Content));
            });
        }
        
        [Test]
        public void HappyPath_CreateEpisodeSocials()
        {
            var episode = fixture.Create<Episode>();
            var result = sut.CreateSocialMediaStub(episode);
            
            Assert.Multiple(() =>
            {
                StringAssert.Contains("<SOCIALS> EPISODE", result);
                StringAssert.Contains($"{episode.EpisodeNumber}", result);
                StringAssert.Contains($"Facebook (najlepiej własne #)", result);

            });
        }
        
        [Test]
        public void HappyPath_CreateSocialsFormated_HasBody()
        {
            var episode = fixture.Create<Episode>();
            var result = sut.CreateSocialDescriptionFormated(episode);
            
            Assert.Multiple(() =>
            {
                Assert.That(result[0].Content, Contains.Substring("<SOCIALS>"));
                Assert.That(result[0].Content, Contains.Substring($"{episode.EpisodeNumber}"));
                Assert.That(result[0].ContentStyle, Is.EqualTo(ContentStyle.HEADING_3));

                Assert.That(result[1].Content, Contains.Substring("Facebook (najlepiej własne #)"));
                Assert.That(result[1].ContentStyle, Is.EqualTo(ContentStyle.NORMAL_TEXT));
            });
        }
        
        [Test]
        public void EdgaCase_HappyPath_CreateEpisode_NoArticles_StillWorks()
        {
            var episode = fixture.Create<Episode>();
            episode.Articles = new List<Article>();
            var result = sut.CreateEpisodeDescription(episode);
            
            Assert.Multiple(() =>
            {
                StringAssert.StartsWith("<TITTLE> 🍵 📰 ITea Morning", result);
                StringAssert.Contains($"{episode.EpisodeNumber}", result);
            });
        }
        
        [Test]
        public void EdgaCase_HappyPath_CreateEpisode_NoEpisode_StillWorks()
        {
            var episode = new Episode();
            
            var result = sut.CreateEpisodeDescription(episode);
            
            StringAssert.StartsWith("<TITTLE> 🍵 📰 ITea Morning #Number", result);

        }
        
        [Test]
        public void HappyPath_CreateEpisodes()
        {
            var episodes = fixture.Create<List<Episode>>();
            var result = sut.CreateEpisodesDescription(episodes);

            Assert.Multiple(() =>
            {
                StringAssert.StartsWith("<WEEKSTART>", result);
                episodes.ForEach(e=> StringAssert.Contains($"<TITTLE> 🍵 📰 ITea Morning {e.EpisodeNumber}",result));
                episodes[0].Articles.ForEach(a => StringAssert.Contains(a.Link, result));
                episodes[0].Articles.ForEach(a => StringAssert.Contains(a.Title, result));
            });
        }
        
        [Test]
        public void Edge_CreateEpisodes_EmptyList_ThrowsError()
        {
            var episodes = new List<Episode>();
            Assert.Throws<ArgumentException>(()=>
            {
                sut.CreateEpisodesDescription(episodes);
            });
        }
        
        [Test]
        public void Edge_CreateEpisodesWithFormating_EmptyList_ThrowsError()
        {
            var episodes = new List<Episode>();
            Assert.Throws<ArgumentException>(()=>
            {
                sut.CreateEpisodesDescriptionWithFormating(episodes);
            });
        }
        
        [Test]
        public void HappyPath_CreateEpisodesWithFormating_ShouldStartWithWeekHeader()
        {
            var episodes = fixture.Create<List<Episode>>();
            var result = sut.CreateEpisodesDescriptionWithFormating(episodes);

            Assert.Multiple(() =>
            {
                Assert.That(result.First().Content, Is.EqualTo("<WEEKSTART>"));
                Assert.That(result.First().ContentStyle, Is.EqualTo(ContentStyle.HEADING_1));
                
            });
        }
    }
}