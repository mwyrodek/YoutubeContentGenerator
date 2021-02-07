using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Authentication;
using YCG.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WordPressPCL;
using WordPressPCL.Models;
using YoutubeContentGenerator.Settings;
using YoutubeContentGenerator.WeeklySummuryGenerator.WordPressWrapper;

namespace YoutubeContentGenerator.WeeklySummuryGenerator
{
    public class APIWeeklySummaryGenerator : IWeeklySummaryGenerator
    {
        private WeeklySummaryPost post;
        private readonly ILogger logger;
        private readonly string username;
        private readonly string password;
        private readonly string blogUrl;
        private readonly string category;
        private IWordPressClientWrapper wrapper;

        public APIWeeklySummaryGenerator(IWordPressClientWrapper wrapper, ILogger<APIWeeklySummaryGenerator> logger,
            IOptions<WordPressOptions> options)
        {
            this.logger = logger;
            this.wrapper = wrapper;

            username = options.Value.BlogLogin;
            password = options.Value.BlogPassword;
            category = options.Value.BlogCategoryName;
            blogUrl = $"{options.Value.BlogUrl}/wp-json/";
            wrapper
                .CreateClient(blogUrl)
                .Authenticate(username, password);
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

            var date = Dates.GetNextWeekSaturday();
            logger.LogTrace($"Scheduling post for {date}");

#if !TEST
            wrapper.Post(post, category, date);
#else
            logger.LogInformation("Test Run - pretending to save episode");
#endif
        }
    }
}