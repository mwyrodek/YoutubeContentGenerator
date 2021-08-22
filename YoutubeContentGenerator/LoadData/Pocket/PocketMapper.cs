using System;
using System.Collections.Generic;
using System.Text;
using PocketSharp;
using PocketSharp.Models;
using YCG.Models;

namespace YoutubeContentGenerator.LoadData.Pocket
{
    public static class PocketMapper
    {
        public static Article Map(PocketItem item)
        {
            return new Article()
            {
                Title = item.Title,
                Link = item.Uri.ToString(),
                Tags = MapTags(item.Tags)
            };
        }

        public static Article Map(PocketArticle pocketArticle)
        {
            return new Article()
            {
                Title = pocketArticle.Title,
                Link = pocketArticle.Uri.ToString(),

            };
        }

        private static List<string> MapTags(IEnumerable<PocketTag> tags)
        {
            var list = new List<string>();
            foreach (var pocketTag in tags)
            {
                list.Add(pocketTag.Name);
            }

            return list;
        }
    }
}
