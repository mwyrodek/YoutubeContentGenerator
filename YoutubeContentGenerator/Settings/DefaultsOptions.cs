namespace YoutubeContentGenerator.Settings
{
    public class DefaultsOptions    {
        public const string Defaults = "Defaults";
        
        public string DefaultMindMapPath { get; set; } 
        public string DefaultDesciriptionLocation { get; set; } 
        public string DefaultDesciriptionFileName { get; set; } 
        public string DefaultWeeklySummaryLocation { get; set; } 
        public string DefaultWeeklySummaryFileName { get; set; } 
        public string DefaultLastEpNumberFile { get; set; } 
        public string SeasonLength { get; set; } 
    }
}