using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Istio.Tracing.Propagation.Tests
{
    public class IstioHeadersFetcherMiddlewareStartupFilterTests
    {
        [Fact]
        public void Configure_AddsMiddleware_CallsNext()
        {
            var appBuilderMock = new Mock<IApplicationBuilder>();
            appBuilderMock
                .Setup(a => a.Use(It.IsAny<Func<RequestDelegate, RequestDelegate>>()))
                .Verifiable();

            var nextWasCalled = false;
            Action<IApplicationBuilder> next = (builder) =>
            {
                nextWasCalled = true;
            };

            var startupFilter = new IstioHeadersFetcherMiddlewareStartupFilter();
            var action = startupFilter.Configure(next);

            action(appBuilderMock.Object);

            Assert.True(nextWasCalled);

            appBuilderMock.Verify();
        }
    }
}
