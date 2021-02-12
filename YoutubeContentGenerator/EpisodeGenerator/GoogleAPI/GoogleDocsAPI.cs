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

namespace YoutubeContentGenerator.EpisodeGenerator.GoogleAPI
{
    [ExcludeFromCodeCoverage]
    public class GoogleDocsApi : IGoogleDocApi
    {
        private static string[] Scopes = {DocsService.Scope.Documents};
        private static string ApplicationName;
        private static string documentId;
        private static DocsService service;
        private ILogger.ILogger logger;

        public GoogleDocsApi(IOptions<GoogleOptions> option, ILogger<GoogleDocsApi> logger)
        {
            this.logger = logger;
            ApplicationName = option.Value.ApplicationName;
            documentId = option.Value.DocumentId;
        }
        public void Authenticate()
        {
            logger.LogTrace("Authenticating to google api");
            GoogleCredential credential;
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(Scopes);
            }

            service = new DocsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            logger.LogTrace("Authenticated");
        }

        public Document ReadFile()
        {
            DocumentsResource.GetRequest request = service.Documents.Get(documentId);
            
            Document doc = request.Execute();
            return doc;
        }

        public void WriteFile(string content)
        {
            logger.LogTrace("Updating doc");
            var location = GetLastLocationFromFile();
            var insertTextRequest = CreateInsertTextRequest(content, location, out var req);
            req.InsertText = insertTextRequest;
            var requests = new List<Request>() {req};
            
            BatchUpdateDocumentRequest body = new BatchUpdateDocumentRequest();
            body.Requests = requests;
            service.Documents.BatchUpdate(body, documentId).Execute();
            logger.LogTrace("Updating doc - Done");
        }

        private static InsertTextRequest CreateInsertTextRequest(string content, Location location, out Request req)
        {
            var text = new InsertTextRequest();
            text.Text = $"\n{content}";
            text.Location = location;
            req = new Request();
            return text;
        }

        private Location GetLastLocationFromFile()
        {
            var readFile = ReadFile();
            var endIndex = readFile.Body.Content.Last().EndIndex;
            var location = new Location();
            location.Index = endIndex - 1;
            return location;
        }
    }
}