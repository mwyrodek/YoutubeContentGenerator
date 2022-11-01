using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using WordPressPCL.Client;
using WordPressPCL.Models;
using YCG.Models;

namespace YoutubeContentGenerator.EpisodeGenerator
{
    public class EpisodeBuilder
    {
        private Episode episode;
        private List<string> specialTags = new List<string>() {"soft", "hard"};
        public EpisodeBuilder(Episode episode)
        {
            this.episode = episode;
        }
        
        // Remove AuthorTags
        
        public Episode Build()
        {
            return episode;
        }

        //remo
        
        public EpisodeBuilder AggregateTagsFromArticles()
        {
            episode.Tags = new List<string>();
            foreach (var article in episode.Articles)
            {
                episode.Tags.AddRange(article.Tags);
            }

            return this;
        }

        public EpisodeBuilder RemoveRedundantTags()
        {
            episode.Tags = episode.Tags.Distinct().ToList();
            return this;
        }

        public EpisodeBuilder RemoveSpecialTags()
        {
            foreach (var specialTag in specialTags)
            {
                episode.Tags.RemoveAll(n => n.Equals(specialTag, StringComparison.OrdinalIgnoreCase));
            }

            return this;
        }
    }
}