// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace Steeltoe.Management.Endpoint.CloudFoundry
{
    public class CloudFoundryEndpointOptions : AbstractEndpointOptions, ICloudFoundryOptions,
        IOptionsMonitor<CloudFoundryEndpointOptions>
    {
        /// <summary>
        /// The name of the configuration section when accessed through <see cref="IConfiguration"/>
        /// </summary>
        public const string SECTION_NAME = "cloudfoundry";
        private const string MANAGEMENT_INFO_PREFIX = "management:endpoints:cloudfoundry";
        private const string VCAP_APPLICATION_ID_KEY = "vcap:application:application_id";
        private const string VCAP_APPLICATION_CLOUDFOUNDRY_API_KEY = "vcap:application:cf_api";
        private const bool Default_ValidateCertificates = true;

        public CloudFoundryEndpointOptions()
            : base()
        {
            Id = string.Empty;
        }

        public CloudFoundryEndpointOptions(IConfiguration config)
            : base(MANAGEMENT_INFO_PREFIX, config)
        {
            Id = string.Empty;
            ApplicationId = config[VCAP_APPLICATION_ID_KEY];
            CloudFoundryApi = config[VCAP_APPLICATION_CLOUDFOUNDRY_API_KEY];
        }

        public bool ValidateCertificates { get; set; } = Default_ValidateCertificates;

        public string ApplicationId { get; set; }

        public string CloudFoundryApi { get; set; }

        /// <inheritdoc/>
        CloudFoundryEndpointOptions IOptionsMonitor<CloudFoundryEndpointOptions>.CurrentValue => this;

        /// <inheritdoc/>
        CloudFoundryEndpointOptions IOptionsMonitor<CloudFoundryEndpointOptions>.Get(string name) => this;

        /// <inheritdoc/>
        IDisposable IOptionsMonitor<CloudFoundryEndpointOptions>.OnChange(Action<CloudFoundryEndpointOptions, string> listener) => null;
    }
}
