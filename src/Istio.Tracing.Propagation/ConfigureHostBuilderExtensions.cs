using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Istio.Tracing.Propagation;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Hosting
{
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
                services.TryAddTransient<IstioHeadersFetcherMiddleware>();
                services.TryAddTransient<HeadersPropagationDelegatingHandler>();

                // This will setup everything automagically
                services.AddSingleton<IHttpMessageHandlerBuilderFilter, HeadersPropagationMessageHandlerBuilderFilter>();
                services.AddSingleton<IStartupFilter, IstioHeadersFetcherMiddlewareStartupFilter>();
            });
        }

    }
}
