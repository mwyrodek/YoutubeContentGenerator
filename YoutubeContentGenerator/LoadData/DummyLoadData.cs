using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using YCG.Models;

namespace YoutubeContentGenerator.LoadData
{
    [ExcludeFromCodeCoverage]
    public class DummyLoadData : ILoadData
    {
        public List<Episode> Execute()
        {
            var list = new List<Episode>();
            for(var i = 0; i<1; i++)
            {
                var Articles = new List<Article>()
                {
                    new Article() {Title = $"test-{i}",Link= $"https://test-{i}.com" },
                    new Article() {Title = $"dev-{i}",Link= $"https://dev-{i}.com" },
                    new Article() {Title = $"biz-{i}",Link= $"https://biz-{i}.com" },
                    new Article() {Title = $"jok-{i}",Link= $"https://jok-{i}.com" }
                };
                list.Add(
                    new Episode()
                    {
                        EpisodeNumber = i,
                        Articles = Articles
                    });
                
            }
            return list;
        }
    }
}
