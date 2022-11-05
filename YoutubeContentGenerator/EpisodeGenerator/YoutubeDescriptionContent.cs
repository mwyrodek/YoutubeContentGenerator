using System;
using System.Collections.Generic;
using System.Text;
using YCG.Models;

namespace YoutubeContentGenerator.EpisodeGenerator
{
    public class YoutubeDescriptionContent : IYoutubeDescriptionContent
    {
        
        public string CreateEpisodesDescription(List<Episode> episodes)
        {
            if (episodes.Count == 0) throw new ArgumentException("list is empty",nameof(episodes));
            var content = new StringBuilder();
            content.Append(YoutubeContentTemplates.WeekStart);
            content.AppendLine();
            content.AppendLine();
            foreach (var episode in episodes)
            {
                content.Append(CreateEpisodeDescription(episode));
                content.Append(CreateSocialMediaStub(episode));
            }

            return content.ToString();
        }

        public List<DescriptionSegments> CreateEpisodesDescriptionWithFormating(List<Episode> episodes)
        {
            throw new NotImplementedException();
        }

        public string CreateEpisodeDescription(Episode episode)
        {
            episode = new EpisodeBuilder(episode)
                .AggregateTagsFromArticles()
                .RemoveRedundantTags()
                .RemoveSpecialTags()
                .Build();
            
            var content = new StringBuilder();

            var title = ReplaceNumber(episode.EpisodeNumber, YoutubeContentTemplates.Title);
            content.Append(title);
            content.AppendLine();
            var description  = ReplaceNumber(episode.EpisodeNumber, YoutubeContentTemplates.Description);
            content.Append(description);
            
            foreach (var article in episode.Articles)
            {
                content.Append($"🔗 {article.Title} {article.Link} \n");
            }
            content.AppendLine();
            content.AppendLine();
            content.Append($"⏲ Timestamps: \n");
            content.Append($"0:00 Intro \n");
            content.AppendLine();
            content.Append($"Tags: \n");
            content.Append(String.Join(", ", episode.Tags));
            content.Append(", IT, ITea, ITea Morning, New, IT News, Wyrodek, Maciej Wyrodek, Maciek Wyrodek, <InsertTags>");
            content.AppendLine();
            content.AppendLine();
            content.Append("======================================================================================================");
            content.AppendLine();
            return content.ToString();
        }

        private static string ReplaceNumber(int number , string content)
        {
            if (number > 0)
            {
                content = content.Replace("#Number", $"{number}");
            }

            return content;
        }

        public string CreateSocialMediaStub(Episode episode)
        {
            episode = new EpisodeBuilder(episode)
                .AggregateTagsFromArticles()
                .RemoveRedundantTags()
                .RemoveSpecialTags()
                .Build();
            // SocialDescriptions
            // Socials
            var content = new StringBuilder();

            var title = ReplaceNumber(episode.EpisodeNumber, YoutubeContentTemplates.SocialSectionHeader);
            var tempDesctiption = ReplaceNumber(episode.EpisodeNumber, YoutubeContentTemplates.SocialSectionHeader);
            content.Append(title);
            content.AppendLine();
            content.Append(tempDesctiption);
            content.AppendLine();
            content.AppendLine();
            content.Append("======================================================================================================");
            content.AppendLine();
            return content.ToString();
        }
    }
}