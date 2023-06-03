using YoutubeContentGenerator.EpisodeGenerator;

namespace YoutubeContentGenerator.Engine
{
    public interface IEngine
    {
        public void LoadData();
        public void GenerateLinks();
        public void GenerateDescription();
        public void GenerateWeekSummary();
        public void GenerateSpecialEpisodeDescription(SpecialEpisodeType episodeType);
    }
}