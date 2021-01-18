using System.Collections.Generic;
using YCG.Models;

namespace YoutubeContentGenerator.DataExtractor
{
    public interface ITransformData
    {
        List<Episode> TransformJsonToEpisodes(string jsonObject);
    }
}