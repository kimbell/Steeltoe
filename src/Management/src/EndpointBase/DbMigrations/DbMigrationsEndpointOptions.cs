// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace Steeltoe.Management.Endpoint.DbMigrations
{
    public class DbMigrationsEndpointOptions : AbstractEndpointOptions, IDbMigrationsOptions,
        IOptionsMonitor<DbMigrationsEndpointOptions>
    {
        /// <summary>
        /// The name of the configuration section when accessed through <see cref="IConfiguration"/>
        /// </summary>
        public const string SECTION_NAME = "dbmigrations";
        private const string MANAGEMENT_INFO_PREFIX = "management:endpoints:dbmigrations";

        public DbMigrationsEndpointOptions()
        {
            Id = "dbmigrations";
            RequiredPermissions = Permissions.RESTRICTED;
        }

        public DbMigrationsEndpointOptions(IConfiguration config)
            : base(MANAGEMENT_INFO_PREFIX, config)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = "dbmigrations";
            }

            if (RequiredPermissions == Permissions.UNDEFINED)
            {
                RequiredPermissions = Permissions.RESTRICTED;
            }
        }

        public string[] KeysToSanitize => Array.Empty<string>();

        /// <inheritdoc/>
        DbMigrationsEndpointOptions IOptionsMonitor<DbMigrationsEndpointOptions>.CurrentValue => this;

        /// <inheritdoc/>
        DbMigrationsEndpointOptions IOptionsMonitor<DbMigrationsEndpointOptions>.Get(string name) => this;

        /// <inheritdoc/>
        IDisposable IOptionsMonitor<DbMigrationsEndpointOptions>.OnChange(Action<DbMigrationsEndpointOptions, string> listener) => null;
    }
}