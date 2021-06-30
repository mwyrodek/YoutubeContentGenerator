using System.Diagnostics.CodeAnalysis;

namespace YoutubeContentGenerator.Settings
{
    [ExcludeFromCodeCoverage]
    public class CosmosDB
    {
        public const string Cosmos = "CosmosDB";
        public string Uri { get; set; } 
        public string PrimaryKey { get; set; }
    }
}