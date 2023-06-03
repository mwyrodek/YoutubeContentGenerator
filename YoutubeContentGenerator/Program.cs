using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Http;
using YoutubeContentGenerator.Engine;
using YoutubeContentGenerator.EpisodeGenerator;
using YoutubeContentGenerator.EpisodeGenerator.GoogleAPI;
using YoutubeContentGenerator.LinkShortener;
using YoutubeContentGenerator.LoadData;
using YoutubeContentGenerator.LoadData.Pocket;
using YoutubeContentGenerator.SeleniumLinkShortener;
using YoutubeContentGenerator.Settings;
using YoutubeContentGenerator.WeeklySummaryGenerator;
using YoutubeContentGenerator.WeeklySummuryGenerator;
using YoutubeContentGenerator.WeeklySummuryGenerator.WordPressWrapper;

namespace YoutubeContentGenerator
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {
/// <summary>
/// EPisode type
/// Normal (defaylt)
/// Half - halfSpecial
/// Kata - IteaKata
/// Special - SPecial
/// Review - Itea Review
/// </summary>
/// <param name="args"> </param>
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using var serviceScope = host.Services.CreateScope();


            var services = serviceScope.ServiceProvider;

            try
            {
                var myService = services.GetRequiredService<ContentGenerator>();
                await myService.Run();

                Console.WriteLine("Success");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Occured");
                Console.WriteLine(ex.Message);
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {

                    services.AddTransient<ContentGenerator>();
                    services.AddScoped<IDataBaseQuery, CosmosDbQuery>();
                    services.AddScoped<IYoutubeDescriptionContent, YoutubeDescriptionContent>();

                    //Configs:
                    services.Configure<WordPressOptions>(hostContext.Configuration.GetSection(WordPressOptions.WordPress));
                    services.Configure<DefaultsOptions>(hostContext.Configuration.GetSection(DefaultsOptions.Defaults));
                    services.Configure<PocketOptions>(hostContext.Configuration.GetSection(PocketOptions.Pocket));
                    services.Configure<GoogleOptions>(hostContext.Configuration.GetSection(GoogleOptions.Google));
                    services.Configure<YourlsOptions>(hostContext.Configuration.GetSection(YourlsOptions.Yourls));
                    services.Configure<CosmosDB>(hostContext.Configuration.GetSection(CosmosDB.Cosmos));
                    services.AddSingleton(args);
                    //engine is progamcore
                    services.AddScoped<IEngine, CosmosDBEngine>();

#if DUMMYLOADER
                    services.AddScoped<ILoadData, DummyLoadData>();
#else
                    services.AddScoped<ILoadData, LoadDataFromPocket>();
                    services.AddScoped<IPocketFactory, PocketFactory>();
                    services.AddScoped<IPocketConector, PocketConector>();
#endif



                    //verision of link shorener
#if DUMMYSHORTENER
                    services.AddScoped<ILinkShortener, DummyShortener>();
#else
                    services.AddScoped<HttpClient, HttpClient>();
                    services.AddScoped<IYourlsApi, YourlsApi>();
                    services.AddScoped<ILinkShortener, YourlsLinkShortener>();




#endif
#if DUMMYYOUTUBE
                    services.AddScoped<IEpisodeNumberHelper, EpisodeNumberHelperFromTextFile>();
                    services.AddScoped<IYouTubeDescriptionGenerator, YouTubeDescriptionGeneratorDummy>();
#else
                    services.AddScoped<IEpisodeNumberHelper, EpisodeNumberHelperFromTextFile>();
                    services.AddScoped<IYouTubeDescriptionGenerator, GoogleDocYoutubeDescriptionGenerator>();
                    services.AddScoped<IGoogleDocApi, GoogleDocsApi>();
                    services.AddScoped<IYoutubeDescriptionContent, YoutubeDescriptionContent>();
#endif
                    services.AddScoped<IWeeklySummaryGenerator, WeeklySummuryGenerator.DummuWeeklySummaryGenerator>();
                    //disabled cause not working
/*#if DUMMYSUMMARY
                    services.AddScoped<IWeeklySummaryGenerator, WeeklySummuryGenerator.DummuWeeklySummaryGenerator>();
#else

                    services.AddScoped<IWordPressClientWrapper, WordPressClientWrapper>();
                    services.AddScoped<IWeeklySummaryGenerator, ApiWeeklySummaryGenerator>();
#endif*/
                });

    }
}