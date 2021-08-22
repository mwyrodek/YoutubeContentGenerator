using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace YCG.Models
{
    [ExcludeFromCodeCoverage]
    public class Article
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public List<string> Tags { get; set; }
    }
}