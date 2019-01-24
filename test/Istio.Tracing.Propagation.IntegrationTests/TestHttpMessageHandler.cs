using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Istio.Tracing.Propagation.IntegrationTests
{
    public class TestHttpMessageHandler : DelegatingHandler
    {
        public HttpRequestHeaders LastRequestHeaders { get; set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = base.SendAsync(request, cancellationToken);

            LastRequestHeaders = request.Headers;

            return response;
        }
    }
}
