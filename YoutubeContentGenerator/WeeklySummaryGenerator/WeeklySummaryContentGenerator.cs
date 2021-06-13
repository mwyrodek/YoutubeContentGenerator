using System;
using System.Collections.Generic;
using System.Text;
using YCG.Models;
using YoutubeContentGenerator.WeeklySummuryGenerator;

namespace YoutubeContentGenerator.WeeklySummaryGenerator
{
    public static class WeeklySummaryContentGenerator
    {
        private static readonly string Title = $"ITea Morning – Linki z Tygodnia {DateTime.UtcNow.GetNextWeekNumber()}";
        private static readonly string Foreword = @$"
[ITea Morning](https://youtube.com/c/ITeaMorning) jest codzienną serią video na moim kanale YouTube  gdzie omawiam różne ciekawe artykuły ze świata IT.

By ułatwić ich wyszukiwanie w przyszłości raz w tygodniu rzucam tutaj wszystkie linki.

Zachęcam do przeglądania!
";

        private const string TitleLine = "## ITea Morning #";

        private const string Footer = "To wszystko na dziś do zobaczenia za tydzień!";


        public static WeeklySummaryPost CreateWeeklySummaryContent(List<Episode> episodes)
        {
            StringBuilder content = new StringBuilder();

            var post = new WeeklySummaryPost {Title = Title};
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
            post.Body = content.ToString();

            return post;
        }

    }
}
