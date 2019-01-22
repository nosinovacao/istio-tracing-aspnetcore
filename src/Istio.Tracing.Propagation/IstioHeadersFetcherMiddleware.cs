using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Istio.Tracing.Propagation
{
    public class IstioHeadersFetcherMiddleware
    {
        private readonly RequestDelegate next;

        public IstioHeadersFetcherMiddleware(RequestDelegate next)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }
    
        public Task InvokeAsync(HttpContext context, IstioHeadersHolder istioHeaders)
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


        public string GetHeader(HttpContext context, string headerName)
        {
            if (context.Request.Headers.TryGetValue(headerName, out var values))
            {
                return values.FirstOrDefault();
            }

            return null;
        }
    }
}
