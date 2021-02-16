using System.Collections.Generic;
using YCG.Models;

namespace YoutubeContentGenerator
{
    public interface ILinkShortener
    {
        public List<Episode> ShortenAllLinks(List<Episode> episodes);
    }
}