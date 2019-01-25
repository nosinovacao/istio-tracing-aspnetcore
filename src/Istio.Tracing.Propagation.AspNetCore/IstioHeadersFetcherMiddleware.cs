using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Istio.Tracing.Propagation
{
    /// <summary>
    /// Plugs into the ASP.Net core middleware pipeline to extract the istio headers from incoming requests.
    /// </summary>
    public class IstioHeadersFetcherMiddleware
    {
        private readonly RequestDelegate next;

        /// <summary>
        /// Initializes a new instance of the <see cref="IstioHeadersFetcherMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <exception cref="ArgumentNullException">next</exception>
        public IstioHeadersFetcherMiddleware(RequestDelegate next)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        /// <summary>
        /// Extracts the istio headers from the request and adds them to the headers holder.
        /// </summary>
        /// <param name="context">The current http context.</param>
        /// <param name="istioHeaders">The istio headers holder for the current request.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public Task InvokeAsync(HttpContext context, IIstioHeadersHolder istioHeaders)
        {

            istioHeaders.RequestId = GetHeader(context, IstioHeaders.REQUEST_ID);
            istioHeaders.B3TraceId = GetHeader(context, IstioHeaders.B3_TRACE_ID);
            istioHeaders.B3SpanId = GetHeader(context, IstioHeaders.B3_SPAN_ID);
            istioHeaders.B3ParentSpanId = GetHeader(context, IstioHeaders.B3_PARENT_SPAN_ID);
            istioHeaders.B3Sampled = GetHeader(context, IstioHeaders.B3_SAMPLED);
            istioHeaders.B3Flags = GetHeader(context, IstioHeaders.B3_FLAGS);
            istioHeaders.OtSpanContext = GetHeader(context, IstioHeaders.OT_SPAN_CONTEXT);

            return next(context);
        }


        private string GetHeader(HttpContext context, string headerName)
        {
            if (context.Request.Headers.TryGetValue(headerName, out var values))
            {
                var firstValue = values.FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(firstValue))
                    return firstValue;
            }

            return null;
        }
    }
}
