// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Steeltoe.Management
{
    public interface IEndpoint
    {
        string Id { get; }

        bool Enabled { get; }

        IEndpointOptions Options { get; }

        string Path { get; }

        /// <summary>
        /// Gets the list of verbs this enpoint responds to
        /// </summary>
        IEnumerable<string> AllowedVerbs { get; }

        /// <summary>
        /// Gets a value indicating whethere an exact path match is required
        /// </summary>
        bool ExactMatch { get; }
    }

    public interface IEndpoint<out TResult> : IEndpoint
    {
        TResult Invoke();
    }

    public interface IEndpoint<out TResult, in TRequest> : IEndpoint
    {
        TResult Invoke(TRequest arg);
    }
}
