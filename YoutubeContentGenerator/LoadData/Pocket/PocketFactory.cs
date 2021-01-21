﻿using Microsoft.Extensions.Configuration;
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
        private readonly AuthenticationOptions options;

        public PocketFactory(ILogger<PocketFactory> logger, IOptions<AuthenticationOptions> options)
        {
            this.logger = logger;
            this.options = options.Value;
        }

        //configuration["Authentication:BlogLogin"];
        public IPocketClient CreatePocketClient()
        {
            try
            {
                return new PocketClient(
                consumerKey: options.PokectAccessCode,
                callbackUri: options.CallbackUri,
                accessCode: options.PokectAccessCode
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
