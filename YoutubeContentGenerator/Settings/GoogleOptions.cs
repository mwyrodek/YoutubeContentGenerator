using System.Diagnostics.CodeAnalysis;

namespace YoutubeContentGenerator.Settings
{   
    [ExcludeFromCodeCoverage]
    public class GoogleOptions
    {
        public const string Google = "Google";
        
        public string ApplicationName { get; set; } 
        public string DocumentId { get; set; }
    }
}