namespace YoutubeContentGenerator.EpisodeGenerator
{
        public static class YoutubeContentTemplates
        {
                public static string WeekStart { get; } = "<WEEKSTART>";
                public static string StandardTitle { get; } = "<TITTLE> 🍵 📰 ITea Morning #Number";
                public static string KataTitle { get; } = "<TITTLE> 🍵🥋 Itea Kata";
                public static string GamesTitle { get; } = "<TITTLE> 🍵🎮 Gry a ITea";
                public static string SpecialTitle { get; } = "<TITTLE> 🍵🌟 ITea Special";
                public static string HalfTitle { get; } = "<TITTLE> 🍵★ Pół-ITea Special";
                public static string ReviewTitle { get; } = "<TITTLE> 🍵📚 Book Review";

                public static string SocialSectionHeader { get; } = "<SOCIALS> EPISODE #Number";
                public static string GenericSpecialSocialSectionHeader { get; } = "<SOCIALS> Special";
                public static string TimeStamp { get; } = "⏲ Timestamps: ";
                public static string BrandTags { get; } = ", IT, ITea, ITea Morning, New, IT News, Wyrodek, Maciej Wyrodek, Maciek Wyrodek, <InsertTags>";


                public static string Description { get; } = @"<DESCRIPTION>
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
Inspirational Corporate - Motivational Successful Business Day
By:  @Sound Gallery by Dmitry Taras  
Link: https://pixabay.com/music/upbeat-inspirational-corporate-motivational-successful-business-day-123492/
License: http://creativecommons.org/licenses/by/4.0/

🖼️ Grafiki
🎬 Intro - Maciej Wyrodek
🎨 Agenda i Thumbnail - Agnieszka Gawrońska  https://pomagierka.pl/

🖥️ Artykuły z odcinka:";

                public static string GenericSpecialDescription { get; } = @"<DESCRIPTION>
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
Inspirational Corporate - Motivational Successful Business Day
By:  @Sound Gallery by Dmitry Taras  
Link: https://pixabay.com/music/upbeat-inspirational-corporate-motivational-successful-business-day-123492/
License: http://creativecommons.org/licenses/by/4.0/

🖼️ Grafiki
🎬 Intro - Maciej Wyrodek
🎨 Agenda i Thumbnail - Agnieszka Gawrońska  https://pomagierka.pl/
";

                public static string SocialDescriptions { get; } = @"Facebook (najlepiej własne #)
Najnowszy ITea Morning czyli poranna porcja artykułów ze świata IT już jest!
🔗 Link ️
Link w bio ⬆️⬆️

W odcinku #Number:
🔸  
🔸  

#ITeaMorning 

# #IT #ITeaMorning
LinkedIn (max 3 #)
Najnowszy ITea Morning czyli poranna porcja artykułów ze świata IT już jest!

W odcinku #Number:
🔸  
🔸 

🔗 Link
#ITeaMorning #";

                public static string KataSocialDescriptions { get; } = @"Facebook (najlepiej własne #)
Najnowsza ITea Kata czyli seria ciekawych wyzwan z automatyzacji juz jest
🔗 Link
Link w bio ⬆️⬆️

W dzisiejszej Kacie:
🔸  
🔸  

#ITeaMorning 

LinkedIn (max 3 #)
Najnowsza ITea Kata czyli seria ciekawych wyzwan z automatyzacji juz jest

W dzisiejszej Kacie:
🔸  
🔸  

🔗link
#ITeaMorning #";

                public static string ReviewSocialDescriptions { get; } = @"Facebook (najlepiej własne #)
Najnowszy ITea Review juz jest a w nim recenzja <TITTLE>
🔗 Link
Link w bio ⬆️⬆️

# #IT #ITeaMorning
LinkedIn (max 3 #)
Najnowszy ITea Review juz jest a w nim recenzja <TITTLE>

🔗 Link
#ITeaMorning #";

                public static string HalfSocialDescriptions { get; } = @"Facebook (najlepiej własne #)
Najnowszy Pół-Special juz jest a w nim przemyslenia na temat <TITTLE>
🔗 Link️
Link w bio ⬆️⬆️

W odcinku dowiecie się o:
🔸  
🔸 

#ITeaMorning 

LinkedIn (max 3 #)
Najnowszy Pół-Special juz jest a w nim przemyslenia na temat <TITTLE>

W odcinku dowiecie się o:
🔸  
🔸 

🔗 Link
#ITeaMorning #";

                public static string GameSocialDescriptions { get; } = @"Facebook (najlepiej własne #)
Najnowsze Gry a ITea jest już dostepne <TITTLE>
🔗 Link️
Link w bio ⬆️⬆️

W odcinku dowiecie się o tym jak gra GAME ma  się do tematu <xyz>

#ITeaMorning 

LinkedIn (max 3 #)
Najnowsze Gry a ITea jest już dostepne <TITTLE>

W odcinku dowiecie się o tym jak gra GAME ma  się do tematu <xyz>

🔗 Link
#ITeaMorning #";


                public static string SpecialSocialDescriptions { get; } = @"Facebook (najlepiej własne #)
Najnowszy Pełno prawny odcinek specjalny jest już dostępny w nim przemyslenia na temat <TITTLE>
🔗 Link
Link w bio ⬆️⬆️

W odcinku dowiecie się o:
🔸  
🔸 

#ITeaMorning 

LinkedIn (max 3 #)
Najnowszy Pełno prawny odcinek specjalny jest już dostępny w nim przemyslenia na temat <TITTLE>

W odcinku dowiecie się o:
🔸  
🔸 

🔗 Link
#ITeaMorning #";
        }
}