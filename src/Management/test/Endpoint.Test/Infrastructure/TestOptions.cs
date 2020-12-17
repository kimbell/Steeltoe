// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Routing;
using System;

namespace Steeltoe.Management.Endpoint.Test.Infrastructure
{
    /// <summary>
    /// Various options that control test setup
    /// </summary>
    public class TestOptions
    {
        public const string SectionName = "Test";

        public bool EnableCloudFoundry { get; set; }

        public bool EnableAuthentication { get; set; }

        public Action<IEndpointRouteBuilder>? ConfigureEndpoints { get; set; }
    }
}
