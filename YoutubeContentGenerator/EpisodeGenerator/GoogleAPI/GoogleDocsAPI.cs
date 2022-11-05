using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Docs.v1;
using Google.Apis.Docs.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using YoutubeContentGenerator.Settings;
using ILogger = Microsoft.Extensions.Logging;
using Range = Google.Apis.Docs.v1.Data.Range;

namespace YoutubeContentGenerator.EpisodeGenerator.GoogleAPI
{
    [ExcludeFromCodeCoverage]
    public class GoogleDocsApi : IGoogleDocApi
    {
        private static readonly string[] Scopes = {DocsService.Scope.Documents};
        private readonly string ApplicationName;
        private readonly string DocumentId;
        private readonly string CredentialsFile;
        private DocsService service;
        private readonly ILogger.ILogger logger;

        public GoogleDocsApi(IOptions<GoogleOptions> option, ILogger<GoogleDocsApi> logger)
        {
            this.logger = logger;
            ApplicationName = option.Value.ApplicationName;
            DocumentId = option.Value.DocumentId;
            CredentialsFile = option.Value.CredentialsFile;
        }
        public void Authenticate()
        {
            logger.LogTrace("Authenticating to google api");
            GoogleCredential credential;
            using var stream = new FileStream(CredentialsFile, FileMode.Open, FileAccess.Read);
            
            credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(Scopes);
            

            service = new DocsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            logger.LogTrace("Authenticated");
        }

        public Document ReadFile()
        {
            var request = service.Documents.Get(DocumentId);
            
            var doc = request.Execute();
            return doc;
        }

        public void InsertTestAtDocEnd(string content)
        {
            logger.LogTrace("Inserting Text to Google doc");
            var location = GetLastLocationFromFile();

            var req = new Request()
            {
                InsertText = CreateInsertTextRequest(content,location)
                
            };
                
            var requests = new List<Request>() {req};

            var body = new BatchUpdateDocumentRequest
            {
                Requests = requests
            };
            service.Documents.BatchUpdate(body, DocumentId).Execute();
            
            logger.LogTrace("Inserting Text  to Google Doc- Done");
        }

        public void UpdateLastLineStyle(ContentStyle style)
        {
            throw new NotImplementedException();
        }

        private static InsertTextRequest CreateInsertTextRequest(string content, Location location)
        {
            var text = new InsertTextRequest
            {
                Text = content,
                Location = location
            };
    
            return text;
        }

        private Location GetLastLocationFromFile()
        {
            var readFile = ReadFile();
            var endIndex = readFile.Body.Content.Last().EndIndex;
            var location = new Location
            {
                Index = endIndex-1
            };
            return location;
        }
        
        private Range GetLastRangeFromFile()
        {
            var readFile = ReadFile();

            var range = new Range()
            {
                StartIndex = readFile.Body.Content.Last().StartIndex,
                EndIndex = readFile.Body.Content.Last().EndIndex
            };
            return range;
        }
    }
}