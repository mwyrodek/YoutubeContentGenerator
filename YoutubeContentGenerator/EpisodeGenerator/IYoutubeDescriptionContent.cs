using System.Collections.Generic;
using Google.Apis.Http;
using WordPressPCL.Models;
using YCG.Models;

namespace YoutubeContentGenerator.EpisodeGenerator
{
    public interface IYoutubeDescriptionContent
    {
        string CreateEpisodesDescription(List<Episode> episodes);
        List<DescriptionSegments> CreateEpisodesDescriptionWithFormating(List<Episode> episodes);
        List<DescriptionSegments> CreateSpecialEpisodeDescriptionWithFormating(SpecialEpisodeType specialEpisodeType);
        
        string CreateSpecialEpisodeDescription(SpecialEpisodeType specialEpisodeType);
        
    }
}