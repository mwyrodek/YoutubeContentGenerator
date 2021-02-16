using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using YCG.Models;

namespace YoutubeContentGenerator.SeleniumLinkShortener
{
    [ExcludeFromCodeCoverage]
    public class DummyShortener : ILinkShortener
    {
        public List<Episode> ShortenAllLinks(List<Episode> episodes)
        {
            return episodes;
        }
    }
}