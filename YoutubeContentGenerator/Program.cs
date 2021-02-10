using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using YoutubeContentGenerator.Blog;
using YoutubeContentGenerator.Engine;
using YoutubeContentGenerator.EpisodeGenerator;
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
        
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Starting");

            var host = CreateHostBuilder(args).Build();
            using (var serviceScope = host.Services.CreateScope())
            {

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
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<ContentGenerator>();
                    services.AddScoped<IEpisodeNumberHelper, EpisodeNumberHelperFromTextFile>();
                    services.AddScoped<IYouTubeDescriptionGenerator, YouTubeDescriptionGenerator>();
                    
                    //Configs:
                    services.Configure<WordPressOptions>(hostContext.Configuration.GetSection(WordPressOptions.WordPress));
                    services.Configure<DefaultsOptions>(hostContext.Configuration.GetSection(DefaultsOptions.Defaults));
                    services.Configure<PocketOptions>(hostContext.Configuration.GetSection(PocketOptions.Pocket));
#if DUMMYLOADER 
                    services.AddScoped<ILoadData, DummyLoadData>();
#else
                    services.AddScoped<ILoadData, LoadDataFromPocket>();
                    services.AddScoped<IPocketFactory, PocketFactory>();
                    services.AddScoped<IPocketConector, PocketConector>();
#endif

                    //engine is progamcore
                    services.AddScoped<IEngine, SimpleEngine>();

                    //verision of link shorener
#if DUMMYSHORTENER
                    services.AddScoped<ILinkShortener, DummyShortener>();
#else
                    services.AddScoped<ILinkShortener, SeleniumLinkShortener.SeleniumLinkShortener>();
                    services.AddScoped<IQuickLinkPage, DashboardAsQuickLinkPage>();
#endif

                    
#if DUMMYSUMMARY
                    services.AddScoped<IWeeklySummaryGenerator, WeeklySummuryGenerator.DummuWeeklySummaryGenerator>();
#else

                    services.AddScoped<IWordPressClientWrapper, WordPressClientWrapper>();
                    services.AddScoped<IWeeklySummaryGenerator, ApiWeeklySummaryGenerator>();
#endif
#if !DUMMYSUMMARY || !DUMMYSHORTENER
                    services.AddScoped<IWebDriver, ChromeDriver>();
                    services.AddScoped<ILoginPage, LoginPage>();
#endif
                });

    }
}