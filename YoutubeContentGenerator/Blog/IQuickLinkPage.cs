namespace YoutubeContentGenerator.Blog
{
    public interface IQuickLinkPage
    {
        public IQuickLinkPage GoTo();
        public string AddLink(string url);
        void DealWithPopUP();
    }
}