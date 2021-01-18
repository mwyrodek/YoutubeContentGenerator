using PocketSharp;

namespace YoutubeContentGenerator.LoadData.Pocket
{
    public interface IPocketFactory
    {
        IPocketClient CreatePocketClient();
    }
}