using System;
using System.Collections.Generic;
using System.Text;
using YCG.Models;
using YoutubeContentGenerator.EpisodeGenerator.GoogleAPI;

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
            if (episodes.Count == 0) throw new ArgumentException("list is empty",nameof(episodes));
            var list = new List<DescriptionSegments>();
            var descriptionSegments = new DescriptionSegments()
            {
                Content = YoutubeContentTemplates.WeekStart,
                ContentStyle = ContentStyle.HEADING_1
            };
            list.Add(descriptionSegments);
            foreach (var episode in episodes)
            {
                list.AddRange(CreateEpisodeDescriptionFormated(episode));
                list.AddRange(CreateSocialDescriptionFormated(episode));
            }
            return list;
        }

        public string CreateEpisodeDescription(Episode episode)
        {
            var list = CreateEpisodeDescriptionFormated(episode);
            return FlatenFormatedEpisodes(list);
        }

        public List<DescriptionSegments> CreateEpisodeDescriptionFormated(Episode episode)
        {
            var list = new List<DescriptionSegments>();
            var title = new DescriptionSegments()
            {
                Content = ReplaceNumber(episode.EpisodeNumber, YoutubeContentTemplates.Title),
                ContentStyle = ContentStyle.HEADING_2
            };
            list.Add(title);
            list.Add(CreateDescriptionBody(episode));
            return list;
        }

        public List<DescriptionSegments> CreateSocialDescriptionFormated(Episode episode)
        {
            var list = new List<DescriptionSegments>();
            var title = new DescriptionSegments()
            {
                Content = ReplaceNumber(episode.EpisodeNumber, YoutubeContentTemplates.SocialSectionHeader),
                ContentStyle = ContentStyle.HEADING_3
            };
            list.Add(title);
            var content = new StringBuilder();
            content.AppendLine();
            var tempDesctiption = ReplaceNumber(episode.EpisodeNumber, YoutubeContentTemplates.SocialDescriptions);
            content.AppendLine();
            content.Append(tempDesctiption);
            content.AppendLine();
            content.AppendLine();
            content.Append("======================================================================================================");
            content.AppendLine();
            content.AppendLine();
            var socialDesc = new DescriptionSegments()
            {
                Content = content.ToString(),
                ContentStyle = ContentStyle.NORMAL_TEXT
            };
            list.Add(socialDesc);
            return list;
        }
        private DescriptionSegments CreateDescriptionBody(Episode episode)
        {
            episode = new EpisodeBuilder(episode)
                .AggregateTagsFromArticles()
                .RemoveRedundantTags()
                .RemoveSpecialTags()
                .Build();
            
            var content = new StringBuilder();
            content.AppendLine();
            content.AppendLine();
            var description  = ReplaceNumber(episode.EpisodeNumber, YoutubeContentTemplates.Description);
            content.Append(description);
            content.AppendLine();
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

            return new DescriptionSegments()
            {
                Content = content.ToString(),
                ContentStyle = ContentStyle.NORMAL_TEXT
            };
        }

        public string CreateSocialMediaStub(Episode episode)
        {
            var list = CreateSocialDescriptionFormated(episode);
            return FlatenFormatedEpisodes(list);
        }

        private string FlatenFormatedEpisodes(List<DescriptionSegments> list)
        {
            var content = new StringBuilder();
            foreach (var segment in list)
            {
                content.Append(segment.Content);
            }
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
    }
}