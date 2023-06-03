using System.ComponentModel;
using YoutubeContentGenerator.EpisodeGenerator;

namespace YoutubeContentGenerator
{
    public static class CommandLineMapper
    {
        public static SpecialEpisodeType MapArguments(string[] arg)
        {
            if (arg.Length == 0)
                return SpecialEpisodeType.NORMAL;
            switch (arg[0])
            {
                case "HALF":
                case "half":
                case "Half":
                    return SpecialEpisodeType.HALF;
                case "SPECIAL":
                case "special":
                case "Special":
                    return SpecialEpisodeType.SPECIAL;
                case "NORMAL":
                case "normal":
                case "Normal":
                    return SpecialEpisodeType.NORMAL;
                case "KATA":
                case "kata":
                case "Kata":
                    return SpecialEpisodeType.KATA;
                case "REVIEW":
                case "review":
                case "Review":
                    return SpecialEpisodeType.REVIEW;
                case "GAMES":
                case "Games":
                case "games":
                    return SpecialEpisodeType.GAMES;                
                
                default:
                    throw new InvalidEnumArgumentException($"{arg[0]} is not Suported episode type");
            }
            
        }
    }
}