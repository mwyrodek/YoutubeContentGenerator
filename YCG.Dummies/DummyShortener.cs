using System.Collections.Generic;
using YCG.Models;

namespace YoutubeContentGenerator.SeleniumLinkShortener
{
    public class DummyShortener : ILinkShortener
    {
        public List<Episode> ShortenAllLinks(List<Episode> episodes)
        {
            return episodes;
        }
    }
}