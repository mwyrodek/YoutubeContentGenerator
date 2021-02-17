using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using YCG.Models;
using YoutubeContentGenerator.Settings;
using YoutubeContentGenerator.WeeklySummuryGenerator;
using YoutubeContentGenerator.WeeklySummuryGenerator.WordPressWrapper;

namespace YoutubeContentGenerator.WeeklySummaryGenerator
{
    public class ApiWeeklySummaryGenerator : IWeeklySummaryGenerator
    {
        private WeeklySummaryPost post;
        private readonly ILogger logger;
        private readonly string category;
        private readonly IWordPressClientWrapper wrapper;

        public ApiWeeklySummaryGenerator(IWordPressClientWrapper wrapper, ILogger<ApiWeeklySummaryGenerator> logger,
            IOptions<WordPressOptions> options)
        {
            this.logger = logger;
            this.wrapper = wrapper;

            category = options.Value.BlogCategoryName;
            var blogUrl = $"{options.Value.BlogUrl}/wp-json/";
            wrapper
                .CreateClient(blogUrl)
                .Authenticate(options.Value.BlogLogin, options.Value.BlogPassword);
        }

        public void CreateWeeklySummaryDescription(List<Episode> episodes)
        {
            logger.LogTrace("Generating Blog Post Content");
            post = WeeklySummaryContentGenerator.CreateWeeklySummaryContent(episodes);
        }

        public void Save()
        {
            if (Object.Equals(post, default(WeeklySummaryPost)))
            {
                logger.LogWarning("Post is empty skipping operation");
                return;
            }

            var date = DateTime.UtcNow.GetNextWeekSaturday();
            logger.LogTrace($"Scheduling post for {date}");

//#if !TEST
            wrapper.Post(post, category, date);
//#else
            logger.LogInformation("Test Run - pretending to save episode");
//#endif
        }
    }
}