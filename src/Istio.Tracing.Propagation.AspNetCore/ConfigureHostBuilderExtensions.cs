using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Istio.Tracing.Propagation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Hosting
{
    /// <summary>
    /// Adds extensions to the IWebHostBuilder.
    /// </summary>
    public static class ConfigureHostBuilderExtensions
    {
        /// <summary>
        /// Sets up the Propagation of ISTIO headers from input requests to outgoing requests.
        /// </summary>
        /// <param name="hostBuilder">The hostbuilder.</param>
        /// <returns>Returns the configured hostbuilder.</returns>
        public static IWebHostBuilder PropagateIstioHeaders(this IWebHostBuilder hostBuilder)
        {
            if (hostBuilder == null)
                throw new ArgumentNullException(nameof(hostBuilder));

            return hostBuilder.ConfigureServices((services) =>
            {
                services.AddHttpContextAccessor();

                services.TryAddTransient<IstioHeadersFetcherMiddleware>();
                services.TryAddTransient<HeadersPropagationDelegatingHandler>();

                services.TryAddScoped<IIstioHeadersHolder, IstioHeadersHolder>();

                // This will setup everything automagically
                services.AddSingleton<IHttpMessageHandlerBuilderFilter, HeadersPropagationMessageHandlerBuilderFilter>();
                services.AddSingleton<IStartupFilter, IstioHeadersFetcherMiddlewareStartupFilter>();
            });
        }

    }
}
