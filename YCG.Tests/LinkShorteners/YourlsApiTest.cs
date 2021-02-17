using System;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using YoutubeContentGenerator.LinkShortener;
using YoutubeContentGenerator.Settings;

namespace YCG.Tests.LinkShorteners
{
    //how to deal with mocking httpclient: https://gingter.org/2018/07/26/how-to-mock-httpclient-in-your-net-c-unit-tests/
    [TestFixture]
    public class YourlsApiTest
    {
        private IYourlsApi sut;
        private Mock<HttpMessageHandler> httpMessageHandlerMock;
        private IFixture fixture;
        private YourlsOptions options;
        private Mock<IOptions<YourlsOptions>> iOptionsMock;

        private const string SampleAnswer = @"{'url':{'keyword':'3','url':'https:\/\/thebests.kotaku.com\/aa','title':'Aa - Gaming Reviews, News, Tips and More. | Kotaku','date':'2021-02-16 21:08:10','ip':'89.151.39.72'},'status':'success','message':'https:\/\/thebests.kotaku.com\/aa added to database','title':'Aa - Gaming Reviews, News, Tips and More. | Kotaku','shorturl':'https:\/\/wyrodek.pl\/x\/3','statusCode':200}";

        [SetUp]
        public void Setup()
        {
            
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            fixture.Inject(httpMessageHandlerMock);
            
            options = fixture.Create<YourlsOptions>();
            options.Url = "https://test.com.pl/x/";
            iOptionsMock = new Mock<IOptions<YourlsOptions>>();
            iOptionsMock.Setup(o => o.Value).Returns(options);
            fixture.Inject(iOptionsMock);
        }


        [Test]
        public void ShortenUrl_UsesAdressFromOptions()
        {
            var httpClient = SetupMockedHttpClient(HttpStatusCode.OK, SampleAnswer);
            fixture.Inject(httpClient);

            sut = fixture.Create<YourlsApi>();

            var param = "test";
            sut.ShortenUrl(param);
            
            
 
            httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1), // we expected a single external request
                ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get  // we expected a GET request
                        && req.RequestUri.ToString().StartsWith("https://test.com.pl/x/yourls-api.php") // to this uri
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }
        
        [Test]
        public void ShortenUrl_SignatureFromOptionsIsAdded()
        {
            var httpClient = SetupMockedHttpClient(HttpStatusCode.OK, SampleAnswer);
            fixture.Inject(httpClient);
            string code = "123456789";
            options.Signature = code;
            sut = fixture.Create<YourlsApi>();

            var param = "test";
            sut.ShortenUrl(param);

            string expectedQueryFragment = $"signature={code}";
 
            httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1), // we expected a single external request
                ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get  // we expected a GET request
                        && req.RequestUri.ToString().Contains(expectedQueryFragment) // to this uri
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }
        
        [Test]
        public void ShortenUrl_WrongSignature_ErrorIsThrown()
        {
            var httpClient = SetupMockedHttpClient(HttpStatusCode.Forbidden, "{'message':'Please log in','errorCode':403,'callback':''}");
            fixture.Inject(httpClient);
            string code = "123456789";
            options.Signature = code;
            sut = fixture.Create<YourlsApi>();

            var param = "test";
            Assert.Throws<UnauthorizedAccessException>(
                () => sut.ShortenUrl(param));
            
        }
        
        [Test]
        public void ShortenUrl_UnexpectedStatus_ErrorIsThrown()
        {
            var httpClient = SetupMockedHttpClient(HttpStatusCode.NotFound, "{'message':'Please log in','errorCode':403,'callback':''}");
            fixture.Inject(httpClient);
            const string code = "123456789";
            options.Signature = code;
            sut = fixture.Create<YourlsApi>();
        
            var param = "test";
            Assert.Throws<ExternalException>(
                () => sut.ShortenUrl(param));
        }
        
        [Test]
        public void ShortenUrl_ActionIsSetToShortUrl()
        {
            var httpClient = SetupMockedHttpClient(HttpStatusCode.OK, "{'id':1,'value':'1'}");
            fixture.Inject(httpClient);

            sut = fixture.Create<YourlsApi>();

            const string param = "test";
            sut.ShortenUrl(param);

            var expectedQueryFragment = $"action=shorturl";
 
            httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1), // we expected a single external request
                ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get  // we expected a GET request
                        && req.RequestUri.ToString().Contains(expectedQueryFragment) // to this uri
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }
        
        [Test]
        public void ShortenUrl_RequestUrlIsEncoded()
        {
            var httpClient = SetupMockedHttpClient(HttpStatusCode.OK, SampleAnswer);
            fixture.Inject(httpClient);
            sut = fixture.Create<YourlsApi>();

            var param = "http://test.pl/test/org?params&param";
            sut.ShortenUrl(param);

            var expectedQueryFragment = HttpUtility.UrlEncode(param);
 
            httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1), // we expected a single external request
                ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get  // we expected a GET request
                        && req.RequestUri.ToString().Contains(expectedQueryFragment) // to this uri
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }
        
        [Test]
        public void ShortenUrl_ReturnFormatIsSetToJson()
        {
            var httpClient = SetupMockedHttpClient(HttpStatusCode.OK, SampleAnswer);
            fixture.Inject(httpClient);
            sut = fixture.Create<YourlsApi>();

            var param = "http://test.pl/test/org?params&param";
            sut.ShortenUrl(param);

            var expectedQueryFragment = "format=json";
 
            httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1), // we expected a single external request
                ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get  // we expected a GET request
                        && req.RequestUri.ToString().Contains(expectedQueryFragment) // to this uri
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }
        
                
        [Test]
        public void ShortenUrl_RequestUrlIsCorect()
        {
            var asnswer =
                @"{'url':{'keyword':'3','url':'https:\/\/thebests.kotaku.com\/aa','title':'Aa - Gaming Reviews, News, Tips and More. | Kotaku','date':'2021-02-16 21:08:10','ip':'89.151.39.72'},'status':'success','message':'https:\/\/thebests.kotaku.com\/aa added to database','title':'Aa - Gaming Reviews, News, Tips and More. | Kotaku','shorturl':'https:\/\/wyrodek.pl\/x\/3','statusCode':200}";
            var httpClient = SetupMockedHttpClient(HttpStatusCode.OK, asnswer);
            fixture.Inject(httpClient);
            sut = fixture.Create<YourlsApi>();
            var param = "http://test.pl/test/org?params&param";
            
            var result = sut.ShortenUrl(param);
            
            Assert.That(result, Is.EqualTo("https://wyrodek.pl/x/3"));
        }
        
        private HttpClient SetupMockedHttpClient(HttpStatusCode code, string content)
        {
            httpMessageHandlerMock
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = code,
                    Content = new StringContent(content),
                })
                .Verifiable();


            return new HttpClient(httpMessageHandlerMock.Object);

        }
        
    }
    
}