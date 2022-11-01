using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using YoutubeContentGenerator.Engine;

[assembly:InternalsVisibleTo("YCG.Tests")]
namespace YoutubeContentGenerator
{
    public class ContentGenerator
    {
        private readonly ILogger logger;
        private readonly IEngine engine;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchCli"/> class.
        /// </summary>
        /// <param name="logger">logger.</param>
        /// <param name="engine">engine.</param>
        public ContentGenerator(ILogger<ContentGenerator> logger, IEngine engine)
        {
            this.logger = logger;
            this.engine = engine;
        }

        internal async Task Run()
        {
            this.logger.LogInformation("Application {applicationEvent} at {dateTime}", "Started", DateTime.UtcNow);
            this.logger.LogTrace("Starting Data Loading");
            this.engine.LoadData();
            /*this.logger.LogTrace("Data load done Done");
            this.logger.LogTrace("Starting Link Generation");
            this.engine.GenerateLinks();
            this.logger.LogTrace("Done Link Generation");*/
            this.logger.LogTrace("Starting DEsc Generation");
            this.engine.GenerateDescription();
            this.logger.LogTrace("Done  DEsc Generation");
            /*this.logger.LogTrace("Starting Week Summary Generation");
            this.engine.GenerateWeekSummary();
             this.logger.LogTrace("Done Week Summary Generation");*/
             await Task.FromResult(0);
        }
        
    }
}