using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Istio.Tracing.Propagation
{
    public class HeadersPropagationMessageHandlerBuilderFilter : IHttpMessageHandlerBuilderFilter
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<HeadersPropagationDelegatingHandler> delegatingHandlerLogger;

        public HeadersPropagationMessageHandlerBuilderFilter(IHttpContextAccessor httpContextAccessor, ILogger<HeadersPropagationDelegatingHandler> delegatingHandlerLogger)
        {
            this.delegatingHandlerLogger = delegatingHandlerLogger ?? throw new ArgumentNullException(nameof(delegatingHandlerLogger));
            this.httpContextAccessor = httpContextAccessor;
        }

        public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            return (builder) =>
            {
                next(builder);

                builder.AdditionalHandlers.Add(new HeadersPropagationDelegatingHandler(httpContextAccessor, delegatingHandlerLogger));
            };
        }

    }
}
