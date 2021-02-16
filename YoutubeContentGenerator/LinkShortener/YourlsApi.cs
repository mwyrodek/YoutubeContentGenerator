using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using Google.Apis.Docs.v1.Data;
using Google.Apis.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using YoutubeContentGenerator.Settings;

namespace YoutubeContentGenerator.LinkShortener
{
    public class YourlsApi : IYourlsApi
    {
        protected HttpClient client;
        private readonly ILogger<YourlsApi> logger;
        private readonly YourlsOptions options;
        private const string EndpointAddress = "yourls-api.php";
        private StringBuilder request;
        public YourlsApi(HttpClient client, ILogger<YourlsApi> logger, IOptions<YourlsOptions> options)
        {
            this.client = client;
            //this.client = new HttpClient();
            this.logger = logger;
            this.options = options.Value;
            client.BaseAddress = new Uri(this.options.Url);
            request = new StringBuilder();
        }
        
        public string ShortenUrl(string url)
        {
            logger.LogTrace("prepering request");
            //append endpoint
            request.Append(EndpointAddress);
            //startparams
            request.Append("?");
            //add signature (authorization
            request.Append($"signature={options.Signature}");
            //Set action
            request.Append($"&action=shorturl");
            //Set action
            var encodedUrl = HttpUtility.UrlEncode(url);
            request.Append($"&url={encodedUrl}");
            //Set response format
            request.Append($"format=json");
            logger.LogTrace("sending request");
            var result = this.client.GetAsync(request.ToString());
            logger.LogTrace("waiting for answer");
            result.Wait();
            logger.LogTrace("answer recived");
            if (result.Result.StatusCode == HttpStatusCode.Forbidden)
                throw new UnauthorizedAccessException("Wrong Signature provided");
            if (result.Result.StatusCode != HttpStatusCode.OK)
                throw new ExternalException($"Link Shortener returned un expected result status code {result.Result.StatusCode}");
            
            var resultContent = result.Result.Content.ReadAsStringAsync().Result;
            
            //if (resultContent is null) throw new NullReferenceException("Content is null");
            var yourlsResponse = JsonConvert.DeserializeObject<YourlsResponse>(resultContent);
            //throw new System.NotImplementedException();
            return yourlsResponse.Shorturl;
        }
        
         
    }
    
    class Url    {
        public string Keyword { get; set; } 
        public string url { get; set; } 
        public string Title { get; set; } 
        public string Date { get; set; } 
        public string Ip { get; set; } 
    }

    class YourlsResponse    {
        public Url Url { get; set; } 
        public string Status { get; set; } 
        public string Message { get; set; } 
        public string Title { get; set; } 
        public string Shorturl { get; set; } 
        public int StatusCode { get; set; } 
    }


}