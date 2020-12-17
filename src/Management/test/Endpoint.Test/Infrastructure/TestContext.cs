// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using Xunit.Abstractions;

namespace Steeltoe.Management.Endpoint.Test.Infrastructure
{
    /// <summary>
    /// Wraps all dependencies for a single test
    /// </summary>
    public class TestContext : WebApplicationFactory<Startup>
    {
        private readonly TestOutputLoggerProvider _loggerProvider;
        private HttpClient? _httpClient;
        private JsonSerializerOptions? _jsonSerializerOptions;

        /// <summary>
        /// Gets or sets a value indicating whether log lines should be stored for analysis after test run
        /// </summary>
        public bool EnableLogCapture
        {
            get => _loggerProvider.EnableLogCapture;
            set => _loggerProvider.EnableLogCapture = value;
        }

        /// Allows a test to perform changes to the <see cref="IServiceCollection"/>
        /// This must be used before creating the HttpClient
        /// </summary>
        public Action<WebHostBuilderContext, IServiceCollection>? ConfigureServicesForSingleTest { get; set; }

        /// <summary>
        /// Gets or sets an action that can be used to configure configuration for a single test
        /// This must be used before creating the HttpClient
        /// </summary>
        public Action<WebHostBuilderContext, IConfigurationBuilder, Dictionary<string, string>>? ConfigureAppConfigurationForSingleTest { get; set; }

        /// <summary>
        /// Gets the json serializer options that have been configured
        /// </summary>
        public JsonSerializerOptions JsonSerializerOptions
        {
            get
            {
                return _jsonSerializerOptions ??= Services.GetRequiredService<IOptions<JsonOptions>>().Value.JsonSerializerOptions;
            }
        }

        /// <summary>
        /// Gets or sets the name of the environment
        /// </summary>
        public string EnvironmentName { get; set; } = "BuildTest";

        /// <summary>
        /// Initializes a new instance of the <see cref="TestContext"/> class.
        /// </summary>
        /// <param name="output">The output helper</param>
        public TestContext(ITestOutputHelper output)
        {
            _loggerProvider = new TestOutputLoggerProvider(output);
        }

        /// <summary>
        /// Gets httpClient that can be used to make requests against your service
        /// </summary>
        public HttpClient HttpClient
        {
            get
            {
                // make sure that we only have one HttpClient for each test
                return _httpClient ??= CreateClient();
            }
        }

        /// <summary>
        /// Verifies that a message has been logged
        /// </summary>
        /// <param name="logLevel">The log leve</param>
        /// <param name="message">Part of the log message. Compared using Contains</param>
        /// <param name="category">The category</param>
        /// <param name="count">The number of times the message should occur. If null, just makes sure one entry exists</param>
        public void VerifyLog(LogLevel logLevel, string? message = null, string? category = null, int? count = null)
            => _loggerProvider.VerifyLog(logLevel, message, category, count);

        /// <inheritdoc/>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseEnvironment(EnvironmentName)
                .UseDefaultServiceProvider(p => p.ValidateScopes = true)
                .ConfigureServices((ctx, services) =>
                {
                    // allow individual tests to configure services
                    ConfigureServicesForSingleTest?.Invoke(ctx, services);
                })

                // don't use Configure() for setting up middleware here
                // it will prevent middleware in the Startup class from running
                .ConfigureAppConfiguration((ctx, configurationBuilder) =>
                {
                    if (ConfigureAppConfigurationForSingleTest != null)
                    {
                        var values = new Dictionary<string, string>();

                        ConfigureAppConfigurationForSingleTest(ctx, configurationBuilder, values);

                        configurationBuilder.AddInMemoryCollection(values);
                    }
                })
                .ConfigureLogging(l =>
                {
                    l.ClearProviders();
                    l.AddFilter("Microsoft", LogLevel.Warning);
                    l.AddProvider(_loggerProvider);
                });
        }

        /// <inheritdoc/>
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder<Startup>(Array.Empty<string>());
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _loggerProvider?.Dispose();
            }
        }
    }
}
