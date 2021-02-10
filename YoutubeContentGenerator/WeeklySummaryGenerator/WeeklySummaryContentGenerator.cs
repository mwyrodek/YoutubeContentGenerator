using System;
using System.Collections.Generic;
using System.Text;
using YCG.Models;

namespace YoutubeContentGenerator.WeeklySummuryGenerator
{
    public static class WeeklySummaryContentGenerator
    {
        
        private static string builtContent;
        private static readonly string Title = $"ITea Morning – Links of the week {DateTime.UtcNow.GetNextWeekNumber()}";
        private static readonly string Foreword = @$"
On my YouTube channel I have a project called “[ITea Morning](https://youtube.com/c/ITeaMorning)”.
The idea is to make daily short videos with few links to articles for audience to read to morning tea or coffee.

Since my channel is in polish for your conviniece I am [weekly sharing here](https://wyrodek.pl/category/itea/) all the intresting materials I am talking about

Now Let’s drink our ITea!
";

        private const string TitleLine = "## ITea Morning #";

        private const string Footer = "That is all for today see you next week!";


        public static WeeklySummaryPost CreateWeeklySummaryContent(List<Episode> episodes)
        {
            StringBuilder content = new StringBuilder();

            var post = new WeeklySummaryPost();
            post.Title = Title;
        content.AppendLine(Foreword);
            foreach (var episode in episodes)
            {
                content.AppendLine($"{TitleLine}{episode.EpisodeNum}");
                foreach (var article in episode.Articles)
                {
                    content.AppendLine($"• [{article.Title}]({article.Link})");
                }

                content.AppendLine();
            }
            content.AppendLine(Footer);
            builtContent = content.ToString();
            post.Body = builtContent;

            return post;
        }

    }
}
