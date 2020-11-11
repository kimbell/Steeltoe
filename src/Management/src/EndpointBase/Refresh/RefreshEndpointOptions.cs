// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace Steeltoe.Management.Endpoint.Refresh
{
    public class RefreshEndpointOptions : AbstractEndpointOptions, IRefreshOptions,
        IOptionsMonitor<RefreshEndpointOptions>
    {
        /// <summary>
        /// The name of the configuration section when accessed through <see cref="IConfiguration"/>
        /// </summary>
        public const string SECTION_NAME = "refresh";
        private const string MANAGEMENT_INFO_PREFIX = "management:endpoints:refresh";

        public RefreshEndpointOptions()
            : base()
        {
            Id = "refresh";
            RequiredPermissions = Permissions.RESTRICTED;
        }

        public RefreshEndpointOptions(IConfiguration config)
            : base(MANAGEMENT_INFO_PREFIX, config)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = "refresh";
            }

            if (RequiredPermissions == Permissions.UNDEFINED)
            {
                RequiredPermissions = Permissions.RESTRICTED;
            }
        }

        public RefreshEndpointOptions CurrentValue => this;

        public RefreshEndpointOptions Get(string name) => this;

        public IDisposable OnChange(Action<RefreshEndpointOptions, string> listener) => null;
    }
}
