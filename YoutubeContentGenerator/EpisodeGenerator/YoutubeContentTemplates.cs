namespace YoutubeContentGenerator.EpisodeGenerator
{
    public static class YoutubeContentTemplates
    {
        public static string WeekStart { get; } = "<WEEKSTART>";
        public static string Title { get; } = "<TITTLE> 🍵 📰 ITea Morning #Number";
        public static string SocialSectionHeader { get; } = "<SOCIALS> EPISODE #Number";
        public static string Description { get; } = @"<<DESCRIPTION>
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

        public static string SocialDescriptions { get; } = @"Facebook (najlepiej własne #)
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
    }
}