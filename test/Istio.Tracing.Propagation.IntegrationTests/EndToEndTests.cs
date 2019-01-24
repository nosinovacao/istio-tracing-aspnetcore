using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Istio.Tracing.Propagation.IntegrationTests
{
    public class EndToEndTests : IClassFixture<FakeWebApplicationFactory>
    {
        private readonly FakeWebApplicationFactory factory;

        public EndToEndTests(FakeWebApplicationFactory factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task HeadersInRequest_ArePropagated()
        {
            var testHandler = new TestHttpMessageHandler();
            var client = factory.WithWebHostBuilder((builder) =>
            {
                builder.ConfigureServices((services) =>
                {
                    services.AddSingleton(testHandler);
                });
            }).CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "/")
            {
                Headers =
                {
                    { IstioHeaders.REQUEST_ID, "1" } ,
                    { IstioHeaders.B3_TRACE_ID, "4" } ,
                    { IstioHeaders.B3_SPAN_ID, "7" } ,
                    { IstioHeaders.B3_PARENT_SPAN_ID, "10" } ,
                    { IstioHeaders.B3_SAMPLED, "13" } ,
                    { IstioHeaders.B3_FLAGS, "16" } ,
                    { IstioHeaders.OT_SPAN_CONTEXT, "19" }
                }
            };

            var response = await client.SendAsync(request);

            Assert.NotNull(testHandler.LastRequestHeaders);

            var headers = testHandler.LastRequestHeaders;

            Assert.Equal("1", headers.GetValues(IstioHeaders.REQUEST_ID).FirstOrDefault());
            Assert.Equal("4", headers.GetValues(IstioHeaders.B3_TRACE_ID).FirstOrDefault());
            Assert.Equal("7", headers.GetValues(IstioHeaders.B3_SPAN_ID).FirstOrDefault());
            Assert.Equal("10", headers.GetValues(IstioHeaders.B3_PARENT_SPAN_ID).FirstOrDefault());
            Assert.Equal("13", headers.GetValues(IstioHeaders.B3_SAMPLED).FirstOrDefault());
            Assert.Equal("16", headers.GetValues(IstioHeaders.B3_FLAGS).FirstOrDefault());
            Assert.Equal("19", headers.GetValues(IstioHeaders.OT_SPAN_CONTEXT).FirstOrDefault());
        }
    }
}
