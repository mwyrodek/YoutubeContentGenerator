using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using YoutubeContentGenerator.Engine;

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
        /// <param name="configuration">configuration.</param>
        public ContentGenerator(ILogger<ContentGenerator> logger, IEngine engine, IConfiguration configuration)
        {
            this.logger = logger;
            this.engine = engine;
        }

        internal async Task Run()
        {
            this.logger.LogInformation("Application {applicationEvent} at {dateTime}", "Started", DateTime.UtcNow);
            
            this.engine.LoadData();
            this.engine.GenerateLinks();
            this.engine.GenerateDescription();
            this.engine.GenerateWeekSummary();
            
        }
        
    }
}