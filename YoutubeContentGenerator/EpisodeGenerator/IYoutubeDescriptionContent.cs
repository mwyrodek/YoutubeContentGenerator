using System.Collections.Generic;
using YCG.Models;

namespace YoutubeContentGenerator.EpisodeGenerator
{
    public interface IYoutubeDescriptionContent
    {
        string CreateEpisodesDescription(List<Episode> episodes);
        string CreateEpisodeDescription(Episode episode);
    }
}