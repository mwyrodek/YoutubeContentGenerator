using System.Collections.Generic;
using YCG.Models;

namespace YoutubeContentGenerator.Engine
{
    public interface IDataBaseQuery
    {
        int GetLastEpisodeNumber();

        void PushEpisode(List<Episode> episodes);
    }
}