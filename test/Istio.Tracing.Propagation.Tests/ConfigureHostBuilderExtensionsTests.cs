using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Istio.Tracing.Propagation.Tests
{
    public class ConfigureHostBuilderExtensionsTests
    {
        [Fact]
        public void UseCorrelation_AddsAllServices()
        {
            var hostBuilder = new Mock<IWebHostBuilder>();

            Action<IServiceCollection> configActionPassed = null;

            hostBuilder
                .Setup(b => b.ConfigureServices(It.IsAny<Action<IServiceCollection>>()))
                .Callback<Action<IServiceCollection>>((configAction) =>
                {
                    configActionPassed = configAction;
                });

            hostBuilder.Object.PropagateIstioHeaders();

            var serviceCollection = new MockServiceCollection();
            configActionPassed(serviceCollection);

            Assert.Equal(6, serviceCollection.Count);
            Assert.Equal(1, serviceCollection.Count(d => d.ServiceType == typeof(IstioHeadersFetcherMiddleware) && d.Lifetime == ServiceLifetime.Transient));
            Assert.Equal(1, serviceCollection.Count(d => d.ServiceType == typeof(HeadersPropagationDelegatingHandler) && d.Lifetime == ServiceLifetime.Transient));
            Assert.Equal(1, serviceCollection.Count(d => d.ServiceType == typeof(IIstioHeadersHolder) && d.ImplementationType == typeof(IstioHeadersHolder) && d.Lifetime == ServiceLifetime.Scoped));
            Assert.Equal(1, serviceCollection.Count(d => d.ServiceType == typeof(IStartupFilter) && d.ImplementationType == typeof(IstioHeadersFetcherMiddlewareStartupFilter) && d.Lifetime == ServiceLifetime.Singleton));
            Assert.Equal(1, serviceCollection.Count(d => d.ServiceType == typeof(IHttpMessageHandlerBuilderFilter) && d.ImplementationType == typeof(HeadersPropagationMessageHandlerBuilderFilter) && d.Lifetime == ServiceLifetime.Singleton));
            Assert.Equal(1, serviceCollection.Count(d => d.ServiceType == typeof(IHttpContextAccessor) && d.ImplementationType == typeof(HttpContextAccessor) && d.Lifetime == ServiceLifetime.Singleton));
        }

        [Fact]
        public void UseCorrelation_FailsIfNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                IWebHostBuilder builder = null;
                builder.PropagateIstioHeaders();
            });
        }

        public class MockServiceCollection : List<ServiceDescriptor>, IServiceCollection
        {

        }
    }
}
