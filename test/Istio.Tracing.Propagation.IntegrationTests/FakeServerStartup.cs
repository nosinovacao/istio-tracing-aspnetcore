using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Istio.Tracing.Propagation.IntegrationTests
{
    public class FakeServerStartup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddHttpClient("testClient")
                .AddHttpMessageHandler((provider) =>
                {
                    return provider.GetRequiredService<TestHttpMessageHandler>();
                });
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Use(async (context, next) =>
            {
                var serviceProvider = context.RequestServices;
                var httpClient = serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("testClient");

                await httpClient.GetAsync("http://foo.com/");

                context.Response.StatusCode = 200;
            });
        }
    }
}
