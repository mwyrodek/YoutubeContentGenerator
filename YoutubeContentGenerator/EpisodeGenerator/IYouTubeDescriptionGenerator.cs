using System.Collections.Generic;
using System.IO;
using YCG.Models;

namespace YoutubeContentGenerator.EpisodeGenerator
{
    public interface IYouTubeDescriptionGenerator
    {
        public void CreateEpisodesDescription(List<Episode> episodes);
        public void Save();
    }
}