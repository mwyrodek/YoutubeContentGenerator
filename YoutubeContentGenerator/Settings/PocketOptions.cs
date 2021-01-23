using System.Collections.Generic;

namespace YoutubeContentGenerator.Settings
{
    public class PocketOptions
    {
        public const string Pocket = "Pocket";
        public string PocketConsumerKey { get; set; }
        public string CallbackUri { get; set; }
        public string PokectAccessCode { get; set; }
        public List<string> Tags { get; set; }
        public int SeasonLength { get; set; }
    }
}