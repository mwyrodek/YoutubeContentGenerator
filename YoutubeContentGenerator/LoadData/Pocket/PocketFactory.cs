using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PocketSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using YoutubeContentGenerator.Settings;

namespace YoutubeContentGenerator.LoadData.Pocket
{
    public class PocketFactory : IPocketFactory
    {
        private readonly ILogger logger;
        private readonly PocketOptions options;

        public PocketFactory(ILogger<PocketFactory> logger, IOptions<PocketOptions> options)
        {
            this.logger = logger;
            this.options = options.Value;
        }

        public IPocketClient CreatePocketClient()
        {
            logger.LogTrace("Creating Pocket Client");
            return new PocketClient(
                consumerKey: options.PocketConsumerKey,
                callbackUri: options.CallbackUri,
                accessCode: options.PokectAccessCode
            );
        }
    }
}