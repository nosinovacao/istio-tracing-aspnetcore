using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xunit;

namespace Istio.Tracing.Propagation.Tests
{
    public class HeadersPropagationMessageHandlerBuilderFilterTests
    {
        [Fact]
        public void Configure_CallsNext_And_AddsAdditionalDefaultHandler()
        {
            var accessorMock = new Mock<IHttpContextAccessor>();

            var nextWasCalled = false;

            Action<HttpMessageHandlerBuilder> next = (b) => nextWasCalled = true;
            var builder = new Mock<HttpMessageHandlerBuilder>();
            var handlers = new List<DelegatingHandler>();
            builder
                .SetupGet(b => b.AdditionalHandlers)
                .Returns(handlers);

            var logger = NullLogger<HeadersPropagationDelegatingHandler>.Instance;

            var builderFilter = new HeadersPropagationMessageHandlerBuilderFilter(accessorMock.Object, logger);

            var buildMethod = builderFilter.Configure(next);

            buildMethod(builder.Object);

            Assert.Single(handlers);
            Assert.IsType<HeadersPropagationDelegatingHandler>(handlers.First());
            Assert.True(nextWasCalled);
        }

        [Fact]
        public void Configure_ThrowsOnNullNext()
        {
            var accessorMock = new Mock<IHttpContextAccessor>();

            var logger = NullLogger<HeadersPropagationDelegatingHandler>.Instance;

            var builderFilter = new HeadersPropagationMessageHandlerBuilderFilter(accessorMock.Object, logger);

            Assert.Throws<ArgumentNullException>(() =>
            {
                builderFilter.Configure(null);
            });

        }

        [Fact]
        public void Ctor_ThrowsOnNullLogger()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var accessorMock = new Mock<IHttpContextAccessor>();

                new HeadersPropagationMessageHandlerBuilderFilter(accessorMock.Object, null);
            });

        }
    }
}
