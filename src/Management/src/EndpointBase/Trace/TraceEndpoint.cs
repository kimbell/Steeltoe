// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Steeltoe.Management.Endpoint.Trace
{
    public class TraceEndpoint : AbstractEndpoint<List<TraceResult>>
    {
        private readonly ILogger<TraceEndpoint> _logger;
        private readonly ITraceRepository _traceRepo;

        public TraceEndpoint(IOptionsMonitor<TraceEndpointOptions> options, ITraceRepository traceRepository, ILogger<TraceEndpoint> logger = null)
            : base(options.CurrentValue)
        {
            _traceRepo = traceRepository ?? throw new ArgumentNullException(nameof(traceRepository));
            _logger = logger;
        }

        public new ITraceOptions Options
        {
            get
            {
                return options as ITraceOptions;
            }
        }

        public override List<TraceResult> Invoke()
        {
            return DoInvoke(_traceRepo);
        }

        public List<TraceResult> DoInvoke(ITraceRepository repo)
        {
            return repo.GetTraces();
        }
    }
}
