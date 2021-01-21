using System.Collections.Generic;
using System.Text;
using YCG.Models;

namespace YoutubeContentGenerator.WeeklySummuryGenerator
{
    public static class WeeklySummaryContentGenerator
    {
        
        private static string builtContent;
        private static string tittle = $"ITea Morning – Links of the week {Dates.GetNextWeekNumber()}";
        private static  string Foreword = @$"
On my YouTube channel I have a project called “[ITea Morning](https://youtube.com/c/ITeaMorning)”.
The idea is to make daily short videos with few links to articles for audience to read to morning tea or coffee.

Since my channel is in polish for your conviniece I am [weekly sharing here](https://wyrodek.pl/category/itea/) all the intresting materials I am talking about

Now Let’s drink our ITea!
";

        private static string TittleLine = "## ITea Morning #";

        private static string footer = "That is all for today see you next week!";



        public static WeeklySummaryPost CreateWeeklySummaryContent(List<Episode> episodes)
        {
            StringBuilder content = new StringBuilder();

            var post = new WeeklySummaryPost();
            post.Title = tittle;
        content.AppendLine(Foreword);
            foreach (var episode in episodes)
            {
                content.AppendLine($"{TittleLine}{episode.EpisodeNum}");
                foreach (var article in episode.Articles)
                {
                    content.AppendLine($"• [{article.Title}]({article.Link})");
                }

                content.AppendLine();
            }
            content.AppendLine(footer);
            builtContent = content.ToString();
            post.Body = builtContent;

            return post;
        }

    }
}
