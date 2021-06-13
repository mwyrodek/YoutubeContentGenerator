using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace YCG.Models
{
    [ExcludeFromCodeCoverage]
    public class Episode
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public List<Article> Articles { get; set; } = new List<Article>();
        public int EpisodeNumber { get; set; }
    }
}