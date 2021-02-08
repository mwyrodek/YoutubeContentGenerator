namespace YoutubeContentGenerator.Engine
{
    public interface IEpisodeNumberHelper
    {
        int GetLastEpisodeNumber();
        void UpdateLastEpisodeNumber(int number);
    }
}