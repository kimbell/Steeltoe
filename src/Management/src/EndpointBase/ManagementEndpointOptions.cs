// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Steeltoe.Management.Endpoint
{
    public class ManagementEndpointOptions : IManagementOptions,
        IOptionsMonitor<ManagementEndpointOptions>
    {
        /// <summary>
        /// The name of the configuration section when accessed through <see cref="IConfiguration"/>
        /// </summary>
        public const string SECTION_NAME = "management:endpoints";
        private const string DEFAULT_PATH = "/actuator";
        private const string MANAGEMENT_INFO_PREFIX = "management:endpoints";

        public ManagementEndpointOptions()
        {
            Path = DEFAULT_PATH;
            EndpointOptions = new List<IEndpointOptions>();
        }

        public ManagementEndpointOptions(IConfiguration config)
            : this()
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var section = config.GetSection(MANAGEMENT_INFO_PREFIX);
            if (section != null)
            {
                section.Bind(this);
            }
        }

        public bool? Enabled { get; set; }

        public bool? Sensitive { get; set; }

        public string Path { get; set; }

        public List<IEndpointOptions> EndpointOptions { get; set; }

        public bool UseStatusCodeFromResponse { get; set; } = true;

        public ManagementEndpointOptions CurrentValue => this;

        public ManagementEndpointOptions Get(string name) => this;

        public IDisposable OnChange(Action<ManagementEndpointOptions, string> listener) => null;
    }
}
