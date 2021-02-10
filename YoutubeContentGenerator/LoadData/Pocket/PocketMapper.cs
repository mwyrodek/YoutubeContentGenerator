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
                Link = item.Uri.ToString()
            };
        }

        public static Article Map(PocketArticle pocketArticle)
        {
            return new Article()
            {
                Title = pocketArticle.Title,
                Link = pocketArticle.Uri.ToString()
            };
        }
    }
}
