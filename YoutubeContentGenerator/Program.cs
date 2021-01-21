﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using YoutubeContentGenerator.Blog;
using YoutubeContentGenerator.Engine;
using YoutubeContentGenerator.EpisodeGenerator;
using YoutubeContentGenerator.LoadData;
using YoutubeContentGenerator.SeleniumLinkShortener;
using YoutubeContentGenerator.Settings;
using YoutubeContentGenerator.WeeklySummuryGenerator;

namespace YoutubeContentGenerator
{
    public class Program
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
                    services.AddScoped<IYouTubeDescriptionGenerator, YouTubeDescriptionGenerator>();
                    
                    //Configs:
                    services.Configure<WordPressOptions>(hostContext.Configuration.GetSection(WordPressOptions.WordPress));
                    services.Configure<DefaultsOptions>(hostContext.Configuration.GetSection(DefaultsOptions.Defaults));
                    services.Configure<PocketOptions>(hostContext.Configuration.GetSection(PocketOptions.Pocket));
#if DUMMYLOADER 
                    services.AddScoped<ILoadData, DummyLoadData>();
#else
                    /*
                    services.AddScoped<ILoadData, LoadDataFromXmind>();
                    services.AddScoped<ITransformData, TransformXmindData>();
                    services.AddScoped<IExctrectFromArchive, ExtractFromZipArchive>();
                    */
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
                    services.AddScoped<ILinkShortener, SeleniumShortenLinks>();
                    services.AddScoped<IQuickLinkPage, DashboardAsQuickLinkPage>();
                    // services.AddScoped<IQuickLinkPage, QuickLinkPage>();
#endif

                    
#if DUMMYSUMMARY
                    services.AddScoped<IWeeklySummaryGenerator, WeeklySummuryGenerator.DummuWeeklySummaryGenerator>();
#else
                    //services.AddScoped<IWeeklySummeryGenerator, WeeklySummuryGenerator.WeeklySummeryGeneratorTxt>();
//                    services.AddScoped<IWeeklySummaryGenerator, WeeklySummuryGenerator.SeleniumWeeklySummaryGenerator>();
//                    services.AddScoped<IAddPostPage, AddPostPage>();
                    services.AddScoped<IWeeklySummaryGenerator, WeeklySummuryGenerator.APIWeeklySummaryGenerator>();
#endif
#if !DUMMYSUMMARY || !DUMMYSHORTENER
                    services.AddScoped<IWebDriver, ChromeDriver>();
                    services.AddScoped<ILoginPage, LoginPage>();
#endif
                });

    }
}