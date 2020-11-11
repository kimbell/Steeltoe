// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Steeltoe.Management.Endpoint.Trace.Test
{
    internal class TestTraceEndpoint : TraceEndpoint
    {
        public TestTraceEndpoint(IOptionsMonitor<TraceEndpointOptions> options, ITraceRepository traceRepository, ILogger<TraceEndpoint> logger = null)
            : base(options, traceRepository, logger)
        {
        }

        public override List<TraceResult> Invoke()
        {
            return new List<TraceResult>();
        }
    }
}
