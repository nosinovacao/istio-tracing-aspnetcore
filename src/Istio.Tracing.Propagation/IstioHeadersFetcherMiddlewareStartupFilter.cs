using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Istio.Tracing.Propagation
{
    public class IstioHeadersFetcherMiddlewareStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return (builder) =>
            {
                builder.UseMiddleware<IstioHeadersFetcherMiddleware>();

                next(builder);
            };

        }
    }
}
