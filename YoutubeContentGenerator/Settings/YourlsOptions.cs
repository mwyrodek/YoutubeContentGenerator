using System.Diagnostics.CodeAnalysis;

namespace YoutubeContentGenerator.Settings
{
    [ExcludeFromCodeCoverage]
    public class YourlsOptions
    {
        public const string Yourls = "Yourls";
        
        public string Url { get; set; } 
        public string Signature { get; set; }
    }
}