using System;
using System.Collections.Generic;
using WordPressPCL.Models;
using YCG.Models;

namespace YoutubeContentGenerator.EpisodeGenerator.GoogleAPI
{
    public class GoogleDocYoutubeDescriptionGenerator : IYouTubeDescriptionGenerator
    {
        private List<DescriptionSegments> formatedContent;
        private readonly IGoogleDocApi api;
        private readonly IYoutubeDescriptionContent content;
        
        public GoogleDocYoutubeDescriptionGenerator(IYoutubeDescriptionContent content, IGoogleDocApi api)
        {
            this.api = api;
            this.api.Authenticate();
            this.content = content;
        }
        public void CreateEpisodesDescription(List<Episode> episodes)
        {
            formatedContent = content.CreateEpisodesDescriptionWithFormating(episodes);
        }

        public void CreateSpecialEpisodesDescription(SpecialEpisodeType type)
        {
            formatedContent = content.CreateSpecialEpisodeDescriptionWithFormating(type);
        }

        public void Save()
        { 
            foreach (var segment in formatedContent)
           {

               this.api.ClearNewlineStyle();
               this.api.InsertTestAtDocEnd(segment.Content);
               this.api.UpdateLastLineStyle(segment.ContentStyle);
           }
        }
    }
}