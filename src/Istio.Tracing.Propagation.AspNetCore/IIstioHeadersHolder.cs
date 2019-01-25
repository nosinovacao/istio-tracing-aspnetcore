using System;
using System.Collections.Generic;
using System.Text;

namespace Istio.Tracing.Propagation
{
    /// <summary>
    /// Container for the Istio headers.
    /// </summary>
    public interface IIstioHeadersHolder
    {
        /// <summary>
        /// Gets or sets the request identifier.
        /// </summary>
        /// <value>
        /// The request identifier.
        /// </value>
        string RequestId { get; set; }

        /// <summary>
        /// Gets or sets the b3 trace identifier.
        /// </summary>
        /// <value>
        /// The b3 trace identifier.
        /// </value>
        string B3TraceId { get; set; }

        /// <summary>
        /// Gets or sets the b3 span identifier.
        /// </summary>
        /// <value>
        /// The b3 span identifier.
        /// </value>
        string B3SpanId { get; set; }

        /// <summary>
        /// Gets or sets the b3 parent span identifier.
        /// </summary>
        /// <value>
        /// The b3 parent span identifier.
        /// </value>
        string B3ParentSpanId { get; set; }

        /// <summary>
        /// Gets or sets the b3 sampled.
        /// </summary>
        /// <value>
        /// The b3 sampled.
        /// </value>
        string B3Sampled { get; set; }

        /// <summary>
        /// Gets or sets the b3 flags.
        /// </summary>
        /// <value>
        /// The b3 flags.
        /// </value>
        string B3Flags { get; set; }

        /// <summary>
        /// Gets or sets the ot span context.
        /// </summary>
        /// <value>
        /// The ot span context.
        /// </value>
        string OtSpanContext { get; set; }
    }
}
