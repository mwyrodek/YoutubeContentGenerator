using System;
using YCG.Models;

namespace YoutubeContentGenerator.WeeklySummuryGenerator.WordPressWrapper
{
    public interface IWordPressClientWrapper
    {
        IWordPressClientWrapper CreateClient(string blogUrl);
        IWordPressClientWrapper Authenticate(string username, string password);
        IWordPressClientWrapper Post(WeeklySummaryPost post, string category, DateTime publishDate);
    }
}