using System.Diagnostics.CodeAnalysis;

namespace YoutubeContentGenerator.Settings
{
    [ExcludeFromCodeCoverage]
    public class DefaultsOptions    {
        public const string Defaults = "Defaults";
        public string DefaultDesciriptionLocation { get; set; } 
        public string DefaultDesciriptionFileName { get; set; }
        public string DefaultLastEpNumberFile { get; set; }
    }
}