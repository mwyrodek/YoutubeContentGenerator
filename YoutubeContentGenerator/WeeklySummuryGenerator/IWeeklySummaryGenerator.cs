using System.Collections.Generic;
using YCG.Models;

namespace YoutubeContentGenerator.WeeklySummuryGenerator
{
    public interface IWeeklySummaryGenerator
    {
        public void CreateWeeklySummaryDescription(List<Episode> episodes);
        public void Save();
    }
}