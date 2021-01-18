using System.Collections.Generic;

namespace YCG.Models
{
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