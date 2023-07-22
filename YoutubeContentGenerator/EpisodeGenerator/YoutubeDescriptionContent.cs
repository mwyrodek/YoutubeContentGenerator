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
            return FlatenFormatedEpisodes(CreateEpisodesDescriptionWithFormating(episodes));
        }


        public string CreateEpisodeDescription(Episode episode)
        {
            return FlatenFormatedEpisodes(CreateEpisodeDescriptionFormated(episode));
        }

        public string CreateSpecialEpisodeDescription(SpecialEpisodeType specialEpisodeType)
        {

            return FlatenFormatedEpisodes(CreateSpecialEpisodeDescriptionWithFormating(specialEpisodeType));
        }

        public List<DescriptionSegments> CreateSpecialEpisodeDescriptionWithFormating(SpecialEpisodeType specialEpisodeType)
        {

            var list = new List<DescriptionSegments>();
            var title = GetTittleForSpecial(specialEpisodeType);
            list.Add(title);
            list.Add(CreateSpecialDescriptionBody(specialEpisodeType));
            
            //socials
            list.AddRange(CreateSpecialSocialDescriptionFormated(specialEpisodeType));
            return list;
        }

        public List<DescriptionSegments> CreateEpisodesDescriptionWithFormating(List<Episode> episodes)
        {
            if (episodes.Count == 0) throw new ArgumentException("list is empty",nameof(episodes));
            var list = new List<DescriptionSegments>();
            foreach (var episode in episodes)
            {
                list.AddRange(CreateEpisodeDescriptionFormated(episode));
                list.AddRange(CreateSocialDescriptionFormated(episode));
            }
            return list;
        }

        private static List<DescriptionSegments> AddWeekStart(List<DescriptionSegments> list)
        {
            var descriptionSegments = new DescriptionSegments()
            {
                Content = YoutubeContentTemplates.WeekStart,
                ContentStyle = ContentStyle.HEADING_1
            };
            list.Add(descriptionSegments);
            return list;
        }

        public string CreateSocialMediaStub(Episode episode)
        {
            var list = CreateSocialDescriptionFormated(episode);
            return FlatenFormatedEpisodes(list);
        }

        public List<DescriptionSegments> CreateEpisodeDescriptionFormated(Episode episode)
        {
            var list = new List<DescriptionSegments>();
            var title = new DescriptionSegments()
            {
                Content = ReplaceNumber(episode.EpisodeNumber, YoutubeContentTemplates.StandardTitle),
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
        

        private List<DescriptionSegments> CreateSpecialSocialDescriptionFormated(SpecialEpisodeType specialEpisodeType)
        {
            var list = new List<DescriptionSegments>();
            var title = new DescriptionSegments()
            {
                Content = YoutubeContentTemplates.GenericSpecialSocialSectionHeader,
                ContentStyle = ContentStyle.HEADING_3
            };
            list.Add(title);
            var content = new StringBuilder();
            content.AppendLine();

            content.AppendLine();
            content.Append(GetSocailDescriptionForSpecial(specialEpisodeType));
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
        

        private DescriptionSegments CreateSpecialDescriptionBody(SpecialEpisodeType specialEpisodeType)
        {
            var content = new StringBuilder();
            content.AppendLine();
            content.AppendLine();
            content.AppendLine(YoutubeContentTemplates.GenericSpecialDescription);
            content.AppendLine();
            content.AppendLine();
            content.AppendLine(YoutubeContentTemplates.TimeStamp);
            content.Append($"0:00 Zajawka \n");
            content.AppendLine();
            content.Append($"Tags: \n");
            content.Append(YoutubeContentTemplates.BrandTags);
            content.AppendLine();
            content.AppendLine();
            return new DescriptionSegments()
            {
                Content = content.ToString(),
                ContentStyle = ContentStyle.NORMAL_TEXT
            };
        }

        private DescriptionSegments GetTittleForSpecial(SpecialEpisodeType specialEpisodeType)
        {

            var episodeTitle = string.Empty;
            switch (specialEpisodeType)
            {
                case SpecialEpisodeType.HALF:
                    episodeTitle = YoutubeContentTemplates.HalfTitle;
                    break;
                case SpecialEpisodeType.SPECIAL:
                    episodeTitle = YoutubeContentTemplates.SpecialTitle;
                    break;
                case SpecialEpisodeType.REVIEW:
                    episodeTitle = YoutubeContentTemplates.ReviewTitle;
                    break;
                case SpecialEpisodeType.KATA:
                    episodeTitle = YoutubeContentTemplates.KataTitle;
                    break;
                case SpecialEpisodeType.GAMES:
                    episodeTitle = YoutubeContentTemplates.GamesTitle;
                    break;
            }
            var title = new DescriptionSegments()
            {
                Content =  episodeTitle,
                ContentStyle = ContentStyle.HEADING_2
            };
            return title;
        }
            
            private string GetSocailDescriptionForSpecial(SpecialEpisodeType specialEpisodeType)
            {
                switch (specialEpisodeType)
                {
                    case SpecialEpisodeType.HALF:
                        return YoutubeContentTemplates.HalfSocialDescriptions;

                    case SpecialEpisodeType.SPECIAL:
                        return YoutubeContentTemplates.SpecialSocialDescriptions;

                    case SpecialEpisodeType.REVIEW:
                        return YoutubeContentTemplates.ReviewSocialDescriptions;

                    case SpecialEpisodeType.KATA:
                        return YoutubeContentTemplates.KataSocialDescriptions;
                    case SpecialEpisodeType.GAMES:
                        return YoutubeContentTemplates.GameSocialDescriptions;
                    default:
                        throw new ArgumentOutOfRangeException($"unknown value of {specialEpisodeType}");

                }
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
            content.AppendLine(YoutubeContentTemplates.TimeStamp);
            content.Append($"0:00 Meme \n");
            content.AppendLine();
            content.Append($"Tags: \n");
            content.Append(String.Join(", ", episode.Tags));
            content.Append(YoutubeContentTemplates.BrandTags);
            content.AppendLine();
            content.AppendLine();

            return new DescriptionSegments()
            {
                Content = content.ToString(),
                ContentStyle = ContentStyle.NORMAL_TEXT
            };
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