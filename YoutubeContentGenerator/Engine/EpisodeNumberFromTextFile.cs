using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using YoutubeContentGenerator.Settings;

namespace YoutubeContentGenerator.Engine
{
    public class EpisodeNumberHelperFromTextFile : IEpisodeNumberHelper
    {
        private readonly ILogger<EpisodeNumberHelperFromTextFile> logger;
        private readonly IOptions<DefaultsOptions> options;
        public EpisodeNumberHelperFromTextFile(ILogger<EpisodeNumberHelperFromTextFile> logger, IOptions<DefaultsOptions> options)
        {
            this.logger=logger;
            this.options=options;
        }
        public int GetLastEpisodeNumber()
        {
            var num = 0;
            logger.LogTrace($"reading from file {options.Value.DefaultLastEpNumberFile}");
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
            logger.LogTrace($"Number {num} read from file.");
            return num;
        }

        public void UpdateLastEpisodeNumber(int number)
        {
            logger.LogTrace($"Saving new number to file");
            using (StreamWriter writer = new StreamWriter(options.Value.DefaultLastEpNumberFile, false))
            {
                writer.Write(number);
            }
            logger.LogTrace($"Number saved");
        }
    }
}