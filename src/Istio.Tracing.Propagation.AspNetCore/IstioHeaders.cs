using System;

namespace Istio.Tracing.Propagation
{
    /// <summary>
    /// Contains the Istio tracing header names.
    /// </summary>
    public static class IstioHeaders
    {
        /// <summary>
        /// The request identifier
        /// </summary>
        public const string REQUEST_ID = "x-request-id";

        /// <summary>
        /// The b3 trace identifier
        /// </summary>
        public const string B3_TRACE_ID = "x-b3-traceid";

        /// <summary>
        /// The b3 span identifier
        /// </summary>
        public const string B3_SPAN_ID = "x-b3-spanid";

        /// <summary>
        /// The b3 parent span identifier
        /// </summary>
        public const string B3_PARENT_SPAN_ID = "x-b3-parentspanid";
    
        /// <summary>
        /// The b3 sampled
        /// </summary>
        public const string B3_SAMPLED = "x-b3-sampled";

        /// <summary>
        /// The b3 flags
        /// </summary>
        public const string B3_FLAGS = "x-b3-flags";

        /// <summary>
        /// The ot span context
        /// </summary>
        public const string OT_SPAN_CONTEXT = "x-ot-span-context";
    }
}
