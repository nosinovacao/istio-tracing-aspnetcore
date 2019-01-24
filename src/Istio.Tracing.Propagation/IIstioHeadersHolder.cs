using System;
using System.Collections.Generic;
using System.Text;

namespace Istio.Tracing.Propagation
{
    public interface IIstioHeadersHolder
    {
        string RequestId { get; set; }
        string B3TraceId { get; set; }
        string B3SpanId { get; set; }
        string B3ParentSpanId { get; set; }
        string B3Sampled { get; set; }
        string B3Flags { get; set; }
        string OtSpanContext { get; set; }
    }
}
