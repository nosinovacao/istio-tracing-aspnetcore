using System;

namespace Istio.Tracing.Propagation
{
    public static class IstioHeaders
    {
        public const string REQUEST_ID = "x-request-id";
        public const string B3_TRACE_ID = "x-b3-trace-id";
        public const string B3_SPAN_ID = "x-b3-spanid";
        public const string B3_PARENT_SPAN_ID = "x-b3-parentspanid";
        public const string B3_SAMPLED = "x-b3-sampled";
        public const string B3_FLAGS = "x-b3-flags";
        public const string OT_SPAN_CONTEXT = "x-ot-span-context";
    }
}
