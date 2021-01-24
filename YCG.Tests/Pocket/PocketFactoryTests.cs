using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using PocketSharp;
using YoutubeContentGenerator.LoadData.Pocket;
using YoutubeContentGenerator.Settings;

namespace YCG.Tests.Pocket
{
    [TestFixture]
    public class PocketFactoryTests
    {
        [Test]
        public void FactoryCreateSimplePocketCLient()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var options = fixture.Create<PocketOptions>();
            var mockIOption = new Mock<IOptions<PocketOptions>>();
            mockIOption.Setup(o => o.Value).Returns(options);
            fixture.Inject(mockIOption);

            var sut = fixture.Create<PocketFactory>();

            var pocketClient = sut.CreatePocketClient();

            Assert.Multiple(
                () =>
                {
                    Assert.That(pocketClient, Is.TypeOf(typeof(PocketClient)));
                    Assert.That(pocketClient.AccessCode, Is.EqualTo(options.PokectAccessCode));
                    Assert.That(pocketClient.ConsumerKey, Is.EqualTo(options.PocketConsumerKey));
                    Assert.That(pocketClient.CallbackUri, Is.EqualTo(options.CallbackUri));
                });
        }
    }
}