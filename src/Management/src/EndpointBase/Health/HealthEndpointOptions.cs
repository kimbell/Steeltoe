﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Steeltoe.Management.Endpoint.Security;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Steeltoe.Management.Endpoint.Health
{
    public class HealthEndpointOptions : AbstractEndpointOptions, IHealthOptions,
        IOptionsMonitor<HealthEndpointOptions>
    {
        /// <summary>
        /// The name of the configuration section when accessed through <see cref="IConfiguration"/>
        /// </summary>
        public const string SECTION_NAME = "health";
        private const string MANAGEMENT_INFO_PREFIX = "management:endpoints:health";

        public HealthEndpointOptions()
            : base()
        {
            Id = "health";
            RequiredPermissions = Permissions.RESTRICTED;
            ExactMatch = false;

            AddDefaultGroups();
        }

        public HealthEndpointOptions(IConfiguration config)
            : base(MANAGEMENT_INFO_PREFIX, config)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = "health";
            }

            if (RequiredPermissions == Permissions.UNDEFINED)
            {
                RequiredPermissions = Permissions.RESTRICTED;
            }

            if (Claim == null && !string.IsNullOrEmpty(Role))
            {
                Claim = new EndpointClaim
                {
                    Type = ClaimTypes.Role,
                    Value = Role
                };
            }

            ExactMatch = false;

            AddDefaultGroups();
        }

        private void AddDefaultGroups()
        {
            if (!Groups.ContainsKey("liveness"))
            {
                Groups.Add("liveness", new HealthGroupOptions { Include = "liveness" });
            }

            if (!Groups.ContainsKey("readiness"))
            {
                Groups.Add("readiness", new HealthGroupOptions { Include = "readiness" });
            }
        }

        public ShowDetails ShowDetails { get; set; }

        public EndpointClaim Claim { get; set; }

        public string Role { get; set; }

        public Dictionary<string, HealthGroupOptions> Groups { get; set; } = new Dictionary<string, HealthGroupOptions>(StringComparer.InvariantCultureIgnoreCase);

        /// <inheritdoc/>
        HealthEndpointOptions IOptionsMonitor<HealthEndpointOptions>.CurrentValue => this;

        /// <inheritdoc/>
        HealthEndpointOptions IOptionsMonitor<HealthEndpointOptions>.Get(string name) => this;

        /// <inheritdoc/>
        IDisposable IOptionsMonitor<HealthEndpointOptions>.OnChange(Action<HealthEndpointOptions, string> listener) => null;
    }
}
