using System.Collections.Generic;
using YCG.Models;

namespace YoutubeContentGenerator.EpisodeGenerator.GoogleAPI
{
    public class GoogleDocYoutubeDescriptionGenerator : IYouTubeDescriptionGenerator
    {
        private IGoogleDocApi api;
        private string builtContent;
        private readonly IYoutubeDescriptionContent content;
        
        public GoogleDocYoutubeDescriptionGenerator(IYoutubeDescriptionContent content, IGoogleDocApi api)
        {
            this.api = api;
            this.api.Authenticate();
            this.content = content;
        }
        public void CreateEpisodesDescription(List<Episode> episodes)
        {
            builtContent = content.CreateEpisodesDescription(episodes);
        }

        public void Save()
        {
            this.api.WriteFile(builtContent);
        }
    }
}