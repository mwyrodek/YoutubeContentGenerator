using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YCG.Models;

namespace YoutubeContentGenerator.WeeklySummuryGenerator
{
    public class WeeklySummeryGeneratorTxt : IWeeklySummaryGenerator
    {
                

        private string builtContent;

        private readonly IConfiguration configuration;

        public WeeklySummeryGeneratorTxt(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void CreateWeeklySummaryDescription(List<Episode> episodes)
        {
            var post = WeeklySummaryContentGenerator.CreateWeeklySummaryContent(episodes);
            builtContent = $"#{post.Title}\n\n{post.Body}";
        }

        public void Save()
        {
            using (StreamWriter writer = new StreamWriter($"{configuration["Defaults:DefaultWeeklySummaryLocation"]}\\{configuration["Defaults:DefaultWeeklySummaryFileName"]}", false))
            {
                writer.Write(builtContent);
            }
        }
    }
}