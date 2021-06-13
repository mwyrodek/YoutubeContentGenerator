using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using Microsoft.Extensions.Options;
using YCG.Models;
using YoutubeContentGenerator.Settings;

namespace YoutubeContentGenerator.EpisodeGenerator
{
    public class YouTubeDescriptionGeneratorText : IYouTubeDescriptionGenerator
    {
        private readonly DefaultsOptions options;
        private readonly IYoutubeDescriptionContent  content;
        private string builtContent;  

        public YouTubeDescriptionGeneratorText(IOptions<DefaultsOptions> options, IYoutubeDescriptionContent content)
        {
            this.options = options.Value;
            this.content = content;
            builtContent = String.Empty;
        }
        public void CreateEpisodesDescription(List<Episode> episodes)
        {
            builtContent = content.CreateEpisodesDescription(episodes);
        }


        [ExcludeFromCodeCoverage]
        public void Save()
        {
            using (var writer = new StreamWriter($"{this.options.DefaultDesciriptionLocation}\\{this.options.DefaultDesciriptionFileName}", true))
            {
                writer.Write(builtContent);
            }
        }
    }
}