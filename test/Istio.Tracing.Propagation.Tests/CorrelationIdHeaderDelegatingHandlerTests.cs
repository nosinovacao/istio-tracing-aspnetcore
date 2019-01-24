using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Istio.Tracing.Propagation.Tests
{
    public class CorrelationIdHeaderDelegatingHandlerTests
    {
        [Fact]
        public async Task SendAsync_AddsHeaders()
        {
            HttpRequestMessage resultRequest = null;
            var testHandler = new TestHandler((request) =>
            {
                resultRequest = request;
                return Task.FromResult(new HttpResponseMessage());
            });

            var headersHolder = new IstioHeadersHolder
            {
                RequestId = "1",
                B3TraceId = "4",
                B3SpanId = "7",
                B3ParentSpanId = "10",
                B3Sampled = "13",
                B3Flags = "16",
                OtSpanContext = "19"
            };

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(s => s.GetService(typeof(IIstioHeadersHolder)))
                .Returns(headersHolder);

            var accessor = new Mock<IHttpContextAccessor>();
            accessor
                .SetupGet(a => a.HttpContext.RequestServices)
                .Returns(serviceProvider.Object);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://foo.com");

            var handler = new HeadersPropagationDelegatingHandler(accessor.Object, NullLogger<HeadersPropagationDelegatingHandler>.Instance)
            {
                InnerHandler = testHandler
            };
            var invoker = new HttpMessageInvoker(handler);
            var result = await invoker.SendAsync(httpRequestMessage, CancellationToken.None);

            Assert.NotNull(result);
            Assert.NotNull(resultRequest);

            Assert.Equal("1", resultRequest.Headers.GetValues(IstioHeaders.REQUEST_ID).First());
            Assert.Equal("4", resultRequest.Headers.GetValues(IstioHeaders.B3_TRACE_ID).First());
            Assert.Equal("7", resultRequest.Headers.GetValues(IstioHeaders.B3_SPAN_ID).First());
            Assert.Equal("10", resultRequest.Headers.GetValues(IstioHeaders.B3_PARENT_SPAN_ID).First());
            Assert.Equal("13", resultRequest.Headers.GetValues(IstioHeaders.B3_SAMPLED).First());
            Assert.Equal("16", resultRequest.Headers.GetValues(IstioHeaders.B3_FLAGS).First());
            Assert.Equal("19", resultRequest.Headers.GetValues(IstioHeaders.OT_SPAN_CONTEXT).First());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData("\r\n")]
        public async Task SendAsync_HeadersNullOrWhitespace_DoesNothing(string headersValue)
        {
            HttpRequestMessage resultRequest = null;
            var testHandler = new TestHandler((request) =>
            {
                resultRequest = request;
                return Task.FromResult(new HttpResponseMessage());
            });

            var headersHolder = new IstioHeadersHolder
            {
                RequestId = headersValue,
                B3TraceId = headersValue,
                B3SpanId = headersValue,
                B3ParentSpanId = headersValue,
                B3Sampled = headersValue,
                B3Flags = headersValue,
                OtSpanContext = headersValue
            };

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(s => s.GetService(typeof(IIstioHeadersHolder)))
                .Returns(headersHolder);

            var accessor = new Mock<IHttpContextAccessor>();
            accessor
                .SetupGet(a => a.HttpContext.RequestServices)
                .Returns(serviceProvider.Object);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://foo.com");

            var handler = new HeadersPropagationDelegatingHandler(accessor.Object, NullLogger<HeadersPropagationDelegatingHandler>.Instance)
            {
                InnerHandler = testHandler
            };
            var invoker = new HttpMessageInvoker(handler);
            var result = await invoker.SendAsync(httpRequestMessage, CancellationToken.None);

            Assert.NotNull(result);
            Assert.NotNull(resultRequest);

            IEnumerable<string> notUsed;

            Assert.False(resultRequest.Headers.TryGetValues(IstioHeaders.REQUEST_ID, out notUsed));
            Assert.False(resultRequest.Headers.TryGetValues(IstioHeaders.B3_TRACE_ID, out notUsed));
            Assert.False(resultRequest.Headers.TryGetValues(IstioHeaders.B3_SPAN_ID, out notUsed));
            Assert.False(resultRequest.Headers.TryGetValues(IstioHeaders.B3_PARENT_SPAN_ID, out notUsed));
            Assert.False(resultRequest.Headers.TryGetValues(IstioHeaders.B3_SAMPLED, out notUsed));
            Assert.False(resultRequest.Headers.TryGetValues(IstioHeaders.B3_FLAGS, out notUsed));
            Assert.False(resultRequest.Headers.TryGetValues(IstioHeaders.OT_SPAN_CONTEXT, out notUsed));
        }

        public class TestHandler : DelegatingHandler
        {
            private readonly Func<HttpRequestMessage, Task<HttpResponseMessage>> testFunc;

            public TestHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> testFunc)
            {
                this.testFunc = testFunc;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return testFunc(request);
            }
        }
    }
}
