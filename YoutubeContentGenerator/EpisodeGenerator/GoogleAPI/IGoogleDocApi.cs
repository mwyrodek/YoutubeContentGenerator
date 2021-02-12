using Google.Apis.Docs.v1.Data;

namespace YoutubeContentGenerator.EpisodeGenerator.GoogleAPI
{
    public interface IGoogleDocApi
    {
        void Authenticate();
        Document ReadFile();
        void WriteFile(string content);
    }
}