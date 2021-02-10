using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Options;
using YCG.Models;
using YoutubeContentGenerator.Settings;

namespace YoutubeContentGenerator.EpisodeGenerator
{
    public class YouTubeDescriptionGenerator : IYouTubeDescriptionGenerator
    {
        private readonly string description = @"<TITTLE> 🍵 📰 ITea Morning #Number


<DESCRIPTION>
To wszystko i wiele więcej w dzisiejszym 🍵 ITea Morning

🗞️ Subskrybuj Itea!:https://youtube.com/c/ITeaMorning?sub_confirmation=1

☕ Postaw mi herbatę 😊 : https://ko-fi.com/iteamorning

🔗 Gdzie mnie można znaleźć
Twitter: https://twitter.com/maciejwyrodek
GitHub: https://github.com/mwyrodek
Facebook: https://www.facebook.com/MaciejWyrodek.Blog/
LinkedIn: https://www.linkedin.com/in/wyrodek/

🎵 Muzyka:
Easy Lemon (30 second) by Kevin MacLeod
Link: https://incompetech.filmmusic.io/song/3695-easy-lemon-30-second-
License: http://creativecommons.org/licenses/by/4.0/

🖼️ Grafiki
🎬 Intro - Adam Kowalczyk
🎨 Agenda i Thumbnail - Agnieszka Gawrońska  https://pomagierka.pl/

🖥️ Materiały z odcinka:";
        
        private readonly StringBuilder content = new StringBuilder();
        private string buildContent;
        private readonly DefaultsOptions options;

        public YouTubeDescriptionGenerator(IOptions<DefaultsOptions> options)
        {
            this.options = options.Value;
        }
        public void CreateEpisodesDescription(List<Episode> episodes)
        {
            
            foreach (var episode in episodes)
            {
                CreateEpisodeDescription(episode);
                content.AppendLine();
                content.AppendLine();
                content.AppendLine();
                content.Append($"📅 Wydarzenia: \n");
                content.Append($"🔗  <opis><link>: \n");
                content.AppendLine();
                content.AppendLine();
                content.Append($"⏲ Timestamps: \n");
                content.Append($"0:00 Intro \n");
                content.AppendLine();
                content.Append($"Tags: \n");
                content.Append("<InsertTags>");
                content.AppendLine();
                content.AppendLine();
                content.Append("======================================================================================================");
                content.AppendLine();
            }

            buildContent = content.ToString();
        }

        private void CreateEpisodeDescription(Episode episode)
        {
            var tempDesctiption = description;
            if(episode.EpisodeNum>0)
            {
                tempDesctiption= tempDesctiption.Replace("#Number", $"#{episode.EpisodeNum}");
            }
            
            content.Append(tempDesctiption);
            content.AppendLine();
            foreach (var article in episode.Articles)
            {
                content.Append($"🔗 {article.Title} {article.Link} \n");
            }
        }

        public void Save()
        {
            using (StreamWriter writer = new StreamWriter($"{this.options.DefaultDesciriptionLocation}\\{this.options.DefaultDesciriptionFileName}", true))
            {
                writer.Write(buildContent);
            }
        }
    }
}