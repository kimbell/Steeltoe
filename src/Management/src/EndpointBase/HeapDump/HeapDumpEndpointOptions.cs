// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace Steeltoe.Management.Endpoint.HeapDump
{
    public class HeapDumpEndpointOptions : AbstractEndpointOptions, IHeapDumpOptions,
        IOptionsMonitor<HeapDumpEndpointOptions>
    {
        /// <summary>
        /// The name of the configuration section when accessed through <see cref="IConfiguration"/>
        /// </summary>
        public const string SECTION_NAME = "heapdump";
        private const string MANAGEMENT_INFO_PREFIX = "management:endpoints:heapdump";

        public HeapDumpEndpointOptions()
            : base()
        {
            Id = "heapdump";
        }

        public HeapDumpEndpointOptions(IConfiguration config)
            : base(MANAGEMENT_INFO_PREFIX, config)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = "heapdump";
            }
        }

        /// <inheritdoc/>
        HeapDumpEndpointOptions IOptionsMonitor<HeapDumpEndpointOptions>.CurrentValue => this;

        /// <inheritdoc/>
        HeapDumpEndpointOptions IOptionsMonitor<HeapDumpEndpointOptions>.Get(string name) => this;

        /// <inheritdoc/>
        IDisposable IOptionsMonitor<HeapDumpEndpointOptions>.OnChange(Action<HeapDumpEndpointOptions, string> listener) => null;
    }
}
