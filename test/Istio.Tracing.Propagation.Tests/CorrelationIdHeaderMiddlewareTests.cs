using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Istio.Tracing.Propagation.Tests
{
    public class CorrelationIdHeaderMiddlewareTests
    {
        [Fact]
        public async Task InvokeAsync_SetsHeaders_BeforeCallingNext()
        {
            var nextWasCalled = false;
            RequestDelegate next = (c) =>
            {
                nextWasCalled = true;
                return Task.CompletedTask;
            };

            var middleware = new IstioHeadersFetcherMiddleware(next);
            var headers = new MockHeaderDictionary()
            {
                { IstioHeaders.REQUEST_ID, "1" } ,
                { IstioHeaders.B3_TRACE_ID, "4" } ,
                { IstioHeaders.B3_SPAN_ID, "7" } ,
                { IstioHeaders.B3_PARENT_SPAN_ID, "10" } ,
                { IstioHeaders.B3_SAMPLED, "13" } ,
                { IstioHeaders.B3_FLAGS, "16" } ,
                { IstioHeaders.OT_SPAN_CONTEXT, "19" } ,
            };

            var context = new Mock<HttpContext>(MockBehavior.Strict);
            context
                .SetupGet(c => c.Request.Headers)
                .Returns(headers);

            var headersHolder = new IstioHeadersHolder();

            await middleware.InvokeAsync(context.Object, headersHolder);

            Assert.True(nextWasCalled);
            Assert.Equal("1", headersHolder.RequestId);
            Assert.Equal("4", headersHolder.B3TraceId);
            Assert.Equal("7", headersHolder.B3SpanId);
            Assert.Equal("10", headersHolder.B3ParentSpanId);
            Assert.Equal("13", headersHolder.B3Sampled);
            Assert.Equal("16", headersHolder.B3Flags);
            Assert.Equal("19", headersHolder.OtSpanContext);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("\n")]
        public async Task InvokeAsync_IgnoresInvalidHeaders(string headersValue)
        {
            var nextWasCalled = false;
            RequestDelegate next = (c) =>
            {
                nextWasCalled = true;
                return Task.CompletedTask;
            };

            var middleware = new IstioHeadersFetcherMiddleware(next);
            var headers = new MockHeaderDictionary()
            {
                { IstioHeaders.REQUEST_ID, headersValue} ,
                { IstioHeaders.B3_TRACE_ID, headersValue} ,
                { IstioHeaders.B3_SPAN_ID, headersValue } ,
                { IstioHeaders.B3_PARENT_SPAN_ID, headersValue } ,
                { IstioHeaders.B3_SAMPLED, headersValue } ,
                { IstioHeaders.B3_FLAGS, headersValue } ,
                { IstioHeaders.OT_SPAN_CONTEXT, headersValue } ,
            };

            var context = new Mock<HttpContext>(MockBehavior.Strict);
            context
                .SetupGet(c => c.Request.Headers)
                .Returns(headers);

            var headersHolder = new IstioHeadersHolder();

            await middleware.InvokeAsync(context.Object, headersHolder);

            Assert.True(nextWasCalled);
            Assert.Null(headersHolder.RequestId);
            Assert.Null(headersHolder.B3TraceId);
            Assert.Null(headersHolder.B3SpanId);
            Assert.Null(headersHolder.B3ParentSpanId);
            Assert.Null(headersHolder.B3Sampled);
            Assert.Null(headersHolder.B3Flags);
            Assert.Null(headersHolder.OtSpanContext);
        }

        internal class MockDisposable : IDisposable
        {
            bool _disposed = false;

            protected virtual void Dispose(bool disposing)
            {
                if (!_disposed) // only dispose once!
                {
                    if (disposing)
                    {
                        // Not in destructor, OK to reference other objects
                    }
                    // perform cleanup for this object
                }
                _disposed = true;
            }

            public void Dispose()
            {
                Dispose(true);

                // tell the GC not to finalize
                GC.SuppressFinalize(this);
            }

            ~MockDisposable()
            {
                Dispose(false);
            }
        }
    }
}
