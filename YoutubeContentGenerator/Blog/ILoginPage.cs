namespace YoutubeContentGenerator.Blog
{
    public interface ILoginPage
    {
        public ILoginPage GoTo();
        public ILoginPage Login(string user, string password);
    }
}