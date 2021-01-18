using YCG.Models;

namespace YoutubeContentGenerator.LoadData.Pocket
{
    public interface IPocketConector
    {
        public Article MoveArticleFromPocketByTag(string tag);
    }
}