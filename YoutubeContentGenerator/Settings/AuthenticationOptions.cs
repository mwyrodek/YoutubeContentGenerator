namespace YoutubeContentGenerator.Settings
{
    public class AuthenticationOptions
    {
        public const string Authentication = "Authentication";
        
        public string BlogLogin { get; set; } 
        public string BlogPassword { get; set; } 
        public string BlogUrl { get; set; } 
        public string PocketConsumerKey { get; set; } 
        public string CallbackUri { get; set; } 
        public string PokectAccessCode { get; set; }         
    }
}