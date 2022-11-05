using Google.Apis.Docs.v1.Data;

namespace YoutubeContentGenerator.EpisodeGenerator.GoogleAPI
{
    public interface IGoogleDocApi
    {
        void Authenticate();
        void InsertTestAtDocEnd(string content);
        void UpdateLastLineStyle(ContentStyle style);
        
        
    }
}