using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace YCG.Models
{
    [ExcludeFromCodeCoverage]
    public class Episode
    {
        public List<Article> Articles { get; set; } = new List<Article>();
        public int EpisodeNum { get; set; }
    }
}