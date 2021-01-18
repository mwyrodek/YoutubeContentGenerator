using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PocketSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace YoutubeContentGenerator.LoadData.Pocket
{
    public class PocketFactory : IPocketFactory
    {
        private readonly ILogger logger;
        private readonly IConfiguration configuration;

        public PocketFactory(ILogger<PocketFactory> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        //configuration["Authentication:BlogLogin"];
        public IPocketClient CreatePocketClient()
        {
            try
            {
                return new PocketClient(
                consumerKey: configuration["Authentication:PocketConsumerKey"],
                callbackUri: configuration["Authentication:CallbackUri"],
                accessCode: configuration["Authentication:PokectAccessCode"]
                );
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw;
            }

        }
    }
}
