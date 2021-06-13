using System;
using System.Collections.Generic;
using NUnit.Framework;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework.Constraints;
using YCG.Models;
using YoutubeContentGenerator.EpisodeGenerator;

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
                 StringAssert.Contains($"#{episode.EpisodeNumber}", result);
                 episode.Articles.ForEach(a => StringAssert.Contains(a.Link, result));
                 episode.Articles.ForEach(a => StringAssert.Contains(a.Title, result));
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
                StringAssert.Contains($"#{episode.EpisodeNumber}", result);
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
                StringAssert.StartsWith("<TITTLE> 🍵 📰 ITea Morning", result);
                episodes.ForEach(e=> StringAssert.Contains($"#{e.EpisodeNumber}",result));
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
    }
}