using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Istio.Tracing.Propagation
{
    /// <summary>
    /// Responsible for adding the <see cref="HeadersPropagationDelegatingHandler"/> to the HttpClient factory pipeline.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Http.IHttpMessageHandlerBuilderFilter" />
    public class HeadersPropagationMessageHandlerBuilderFilter : IHttpMessageHandlerBuilderFilter
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<HeadersPropagationDelegatingHandler> delegatingHandlerLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeadersPropagationMessageHandlerBuilderFilter"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="delegatingHandlerLogger">The delegating handler logger.</param>
        /// <exception cref="ArgumentNullException">delegatingHandlerLogger</exception>
        public HeadersPropagationMessageHandlerBuilderFilter(IHttpContextAccessor httpContextAccessor, ILogger<HeadersPropagationDelegatingHandler> delegatingHandlerLogger)
        {
            this.delegatingHandlerLogger = delegatingHandlerLogger ?? throw new ArgumentNullException(nameof(delegatingHandlerLogger));
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Applies additional initialization to the <see cref="T:Microsoft.Extensions.Http.HttpMessageHandlerBuilder" />
        /// </summary>
        /// <param name="next">A delegate which will run the next <see cref="T:Microsoft.Extensions.Http.IHttpMessageHandlerBuilderFilter" />.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">next</exception>
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
