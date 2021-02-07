using System.Diagnostics.CodeAnalysis;

namespace YoutubeContentGenerator.Settings
{
    [ExcludeFromCodeCoverage]
    public class WordPressOptions
    {
        public const string WordPress = "WordPress";
        
        public string BlogLogin { get; set; } 
        public string BlogPassword { get; set; } 
        public string BlogUrl { get; set; }
        public string BlogCategoryName { get; set; }
    }
}