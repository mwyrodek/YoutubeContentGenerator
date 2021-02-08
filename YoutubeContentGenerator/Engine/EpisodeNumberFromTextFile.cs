using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using YoutubeContentGenerator.Settings;

namespace YoutubeContentGenerator.Engine
{
    public class EpisodeNumberHelperFromTextFile : IEpisodeNumberHelper
    {
        private ILogger<EpisodeNumberHelperFromTextFile> logger;
        private IOptions<DefaultsOptions> options;
        public EpisodeNumberHelperFromTextFile(ILogger<EpisodeNumberHelperFromTextFile> logger, IOptions<DefaultsOptions> options)
        {
            this.logger=logger;
            this.options=options;
        }
        public int GetLastEpisodeNumber()
        {
            int num = 0;
            using (StreamReader sr = new StreamReader(options.Value.DefaultLastEpNumberFile))
            {
                string line;
                // Read and display lines from the file until the end of
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    num = int.Parse(line);
                }
            }

            return num;
        }

        public void UpdateLastEpisodeNumber(int number)
        {
            using (StreamWriter writer = new StreamWriter(options.Value.DefaultLastEpNumberFile, false))
            {
                writer.Write(number);
            }
        }
    }
}