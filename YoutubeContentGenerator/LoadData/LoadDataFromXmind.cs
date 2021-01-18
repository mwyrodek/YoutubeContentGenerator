using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YCG.Models;
using YoutubeContentGenerator.DataExtractor;
using YoutubeContentGenerator.ExtractDataFromFile;

namespace YoutubeContentGenerator.LoadData
{
    public class LoadDataFromXmind : ILoadData
    {
        private readonly ILogger logger;
        private readonly IConfiguration configuration;
        private readonly IExctrectFromArchive extract;
        private readonly ITransformData dataTransform;
        public LoadDataFromXmind(ILogger<LoadDataFromXmind> logger, IConfiguration configuration, IExctrectFromArchive extract, ITransformData dataTransform)
        {
            this.logger = logger;
            this.extract = extract;
            this.configuration = configuration;
            this.dataTransform = dataTransform;
        }

        public List<Episode> Execute()
        {
            var arichveInfo = new FileInfo(configuration["Defaults:DefaultMindMapPath"]);
            var fileName = "content.json";
            var readFileFromArchive = extract.ReadFileFromArchive(arichveInfo, fileName);
            logger.LogInformation("data Extrated from file");
           var episodes = dataTransform.TransformJsonToEpisodes(readFileFromArchive);
            logger.LogInformation("data deserialized");

            return episodes;
        }
    }
}
