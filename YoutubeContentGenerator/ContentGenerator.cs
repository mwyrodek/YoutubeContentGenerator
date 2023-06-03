using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using YoutubeContentGenerator.Engine;
using YoutubeContentGenerator.EpisodeGenerator;

[assembly:InternalsVisibleTo("YCG.Tests")]
namespace YoutubeContentGenerator
{
    public class ContentGenerator
    {
        private readonly ILogger logger;
        private readonly IEngine engine;
        private readonly string[] args;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchCli"/> class.
        /// </summary>
        /// <param name="logger">logger.</param>
        /// <param name="engine">engine.</param>
        public ContentGenerator(ILogger<ContentGenerator> logger, IEngine engine, string[] args)
        {
            this.logger = logger;
            this.engine = engine;
            this.args = args;
        }

        internal async Task Run()
        {
//if normal go old route if special run only generate description but for special

            this.logger.LogInformation("Application {applicationEvent} at {dateTime}", "Started", DateTime.UtcNow);
            var episodeType = CommandLineMapper.MapArguments(args);
            if (episodeType == SpecialEpisodeType.NORMAL)
            {
                this.logger.LogInformation("Generating Stanard Episode"); 
                GenerateStandardEpisodes();
            }
            else
            {
                this.logger.LogInformation("Generating Special Episode");
                GenerateSpecialEpisodes(episodeType);
            }
            
            this.logger.LogInformation("All Task Done");
            await Task.FromResult(0);
        }

        private void GenerateStandardEpisodes()
        {
            this.logger.LogTrace("Starting Data Loading");
            this.engine.LoadData();
            this.logger.LogTrace("Data load done Done");

            this.logger.LogTrace("Starting Desc Generation");
            this.engine.GenerateDescription();
            this.logger.LogTrace("Done  Desc Generation");
            
        }
        
        private void GenerateSpecialEpisodes(SpecialEpisodeType episodeType)
        {


            this.logger.LogTrace("Starting Desc Generation");
            this.engine.GenerateSpecialEpisodeDescription(episodeType);
            this.logger.LogTrace("Done  Desc Generation");
            
        }
    }
}