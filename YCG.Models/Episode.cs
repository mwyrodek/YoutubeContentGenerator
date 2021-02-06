using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace YCG.Models
{
    [ExcludeFromCodeCoverage]
    public class Episode
    {
        public Episode()
        {
            Articles = new List<Article>();
        }

        public List<Article> Articles { get; set; }
        public int EpisodeNum { get; set; }
    }
}