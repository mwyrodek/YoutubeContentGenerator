namespace YoutubeContentGenerator.Settings
{
    public class WordPressOptions
    {
        public const string WordPress = "WordPress";
        
        public string BlogLogin { get; set; } 
        public string BlogPassword { get; set; } 
        public string BlogUrl { get; set; }
        public string BlogCategoryName { get; set; }
    }
}