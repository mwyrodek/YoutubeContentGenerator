using System;
using System.Collections.Generic;
using System.Text;
using YCG.Models;

namespace YoutubeContentGenerator.EpisodeGenerator
{
    public class YoutubeDescriptionContent : IYoutubeDescriptionContent
    {
        private readonly string weekStart = "<WEEKSTART> \n \n";
        private readonly string description = @"<TITTLE> 🍵 📰 ITea Morning #Number


<DESCRIPTION>
To wszystko i wiele więcej w dzisiejszym 🍵 ITea Morning

🗞️ Subskrybuj Itea!:https://youtube.com/c/ITeaMorning?sub_confirmation=1

☕ Postaw mi herbatę 😊 : https://ko-fi.com/iteamorning

🔗 Gdzie mnie można znaleźć
Twitter: https://twitter.com/maciejwyrodek
GitHub: https://github.com/mwyrodek
Facebook: https://www.facebook.com/MaciejWyrodek.ITea/
LinkedIn: https://www.linkedin.com/in/wyrodek/

🎙️Podcast
Spotify: https://open.spotify.com/show/3Yo5AtGfQVXAxFA5q13zdF
Google Podcast: https://www.google.com/podcasts?feed=aHR0cHM6Ly9hbmNob3IuZm0vcy82NjJlZDEwOC9wb2RjYXN0L3Jzcw==

🎵 Muzyka:
Easy Lemon (30 second) by Kevin MacLeod
Link: https://incompetech.filmmusic.io/song/3695-easy-lemon-30-second-
License: http://creativecommons.org/licenses/by/4.0/

🖼️ Grafiki
🎬 Intro - Adam Kowalczyk
🎨 Agenda i Thumbnail - Agnieszka Gawrońska  https://pomagierka.pl/

🖥️ Artykuły z odcinka:";


        private readonly string socialMediaTemplate = @"Posty sieci społecznościowe
        Facebook (najlepiej własne #)
#ITeaMorning 
        Najnowszy ITea Morning czyli poranna porcja artykułów ze świata IT już jest!
        Link w komentarzu ⬇️⬇️

        W odcinku #Number:
        🔸  
        🔸  

        Twitter
            Najnowszy ITea Morning czyli poranna porcja artykułów ze świata IT już jest!
        link

# #IT #ITeaMorning
            LinkedIn (max 3 #)
        Najnowszy ITea Morning czyli poranna porcja artykułów ze świata IT już jest!

        W odcinku #Number:
        🔸  
        🔸 

        link
#ITeaMorning #";
        
        public string CreateEpisodesDescription(List<Episode> episodes)
        {
            if (episodes.Count == 0) throw new ArgumentException("list is empty",nameof(episodes));
            var content = new StringBuilder();
            content.Append(weekStart);
            foreach (var episode in episodes)
            {
                content.Append(CreateEpisodeDescription(episode));
                content.Append(CreateSocialMediaStub(episode));
            }

            return content.ToString();
        }

        public string CreateEpisodeDescription(Episode episode)
        {
            episode = new EpisodeBuilder(episode)
                .AggregateTagsFromArticles()
                .RemoveRedundantTags()
                .RemoveSpecialTags()
                .Build();
            
            var content = new StringBuilder();
            var tempDesctiption = description;
            if (episode.EpisodeNumber > 0)
            {
                tempDesctiption = tempDesctiption.Replace("#Number", $"{episode.EpisodeNumber}");
            }

            content.Append(tempDesctiption);
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
            content.Append("======================================================================================================");
            content.AppendLine();
            return content.ToString();
        }
        
        public string CreateSocialMediaStub(Episode episode)
        {
            episode = new EpisodeBuilder(episode)
                .AggregateTagsFromArticles()
                .RemoveRedundantTags()
                .RemoveSpecialTags()
                .Build();
            
            var content = new StringBuilder();
            var tempDesctiption = socialMediaTemplate;
            if (episode.EpisodeNumber > 0)
            {
                tempDesctiption = tempDesctiption.Replace("#Number", $"{episode.EpisodeNumber}");
            }

            content.Append(tempDesctiption);
            content.AppendLine();
            content.AppendLine();
            content.Append("======================================================================================================");
            content.AppendLine();
            return content.ToString();
        }
    }
}