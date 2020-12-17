// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.Management.Endpoint.Test.Infrastructure;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.Management.Endpoint.Test.Actuator.Env
{
    public class ActuatorTests
    {
        private readonly ITestOutputHelper _output;

        public ActuatorTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData(true)]
        //[InlineData(false)]
        public async void EndPointIsNotExposed(bool useLegacy)
        {
            using (var tc = new TestContext(_output))
            {
                tc.EnvironmentName = "EnvironmentName";
                tc.ConfigureServicesForSingleTest = (ctx, services) =>
                {
                    if (useLegacy)
                    {
                        Endpoint.Env.EndpointServiceCollectionExtensions.AddEnvActuator(services, ctx.Configuration);
                        services.PostConfigure<TestOptions>(options => options.ConfigureEndpoints = endpoints => endpoints.Map<Endpoint.Env.EnvEndpoint>());
                    }
                };
                tc.ConfigureAppConfigurationForSingleTest = (_, _, values) =>
                {
                    values.Add("management:endpoints:enabled", "true");
                };

                var json = await tc.HttpClient.GetStringAsync("/actuator/env").ConfigureAwait(false);

                Assert.Empty(json);
            }
        }

        [Theory]
        [InlineData(true)]
        //[InlineData(false)]
        public async void EndpointIsExposed(bool useLegacy)
        {
            using (var tc = new TestContext(_output))
            {
                tc.EnvironmentName = "EnvironmentName";
                tc.ConfigureServicesForSingleTest = (ctx, services) =>
                {
                    if (useLegacy)
                    {
                        Endpoint.Env.EndpointServiceCollectionExtensions.AddEnvActuator(services, ctx.Configuration);
                        services.PostConfigure<TestOptions>(options => options.ConfigureEndpoints = endpoints => endpoints.Map<Endpoint.Env.EnvEndpoint>());
                    }
                };
                tc.ConfigureAppConfigurationForSingleTest = (ctx, _, values) =>
                {
                    values.Add("Logging:IncludeScopes", "false");
                    values.Add("Logging:LogLevel:Default", "Warning");
                    values.Add("Logging:LogLevel:Pivotal", "Information");
                    values.Add("Logging:LogLevel:Steeltoe", "Information");
                    values.Add("management:endpoints:enabled", "true");
                    values.Add("management:endpoints:actuator:exposure:include:0", "env");
                };

                var json = await tc.HttpClient.GetStringAsync("/actuator/env").ConfigureAwait(false);

                Assert.Contains("\"activeProfiles\":[\"EnvironmentName\"]", json);
                Assert.Contains("\"Logging:IncludeScopes\":{\"value\":\"false\"}", json);
                Assert.Contains("\"Logging:LogLevel:Default\":{\"value\":\"Warning\"}", json);
                Assert.Contains("\"Logging:LogLevel:Pivotal\":{\"value\":\"Information\"}", json);
                Assert.Contains("\"Logging:LogLevel:Steeltoe\":{\"value\":\"Information\"}", json);
                Assert.Contains("\"management:endpoints:enabled\":{\"value\":\"true\"}", json);
            }
        }

        [Theory]
        [InlineData(true)]
        //[InlineData(false)]
        public async void CloudFoundryEndpoint_ReturnsExpected(bool useLegacy)
        {
            using (var tc = new TestContext(_output))
            {
                tc.EnvironmentName = "EnvironmentName";
                tc.ConfigureServicesForSingleTest = (ctx, services) =>
                {
                    if (useLegacy)
                    {
                        Endpoint.Env.EndpointServiceCollectionExtensions.AddEnvActuator(services, ctx.Configuration);
                        CloudFoundry.EndpointServiceCollectionExtensions.AddCloudFoundryActuator(services, ctx.Configuration);
                        services.PostConfigure<TestOptions>(options => options.ConfigureEndpoints = endpoints =>
                        {
                            endpoints.Map<CloudFoundry.CloudFoundryEndpoint>();
                            endpoints.Map<Endpoint.Env.EnvEndpoint>();
                        });
                    }
                };
                tc.ConfigureAppConfigurationForSingleTest = (_, _, values) =>
                {
                    values.Add("Logging:IncludeScopes", "false");
                    values.Add("Logging:LogLevel:Default", "Warning");
                    values.Add("Logging:LogLevel:Pivotal", "Information");
                    values.Add("Logging:LogLevel:Steeltoe", "Information");
                    values.Add("management:endpoints:enabled", "true");
                };

                var json = await tc.HttpClient.GetStringAsync("/cloudfoundryapplication/env").ConfigureAwait(false);

                Assert.Contains("\"activeProfiles\":[\"EnvironmentName\"]", json);
                Assert.Contains("\"Logging:IncludeScopes\":{\"value\":\"false\"}", json);
                Assert.Contains("\"Logging:LogLevel:Default\":{\"value\":\"Warning\"}", json);
                Assert.Contains("\"Logging:LogLevel:Pivotal\":{\"value\":\"Information\"}", json);
                Assert.Contains("\"Logging:LogLevel:Steeltoe\":{\"value\":\"Information\"}", json);
                Assert.Contains("\"management:endpoints:enabled\":{\"value\":\"true\"}", json);
                Assert.Contains("\"applicationName\":{\"value\":\"Steeltoe.Management.Endpoint.Test\"}", json);
            }
        }

        [Theory]
        [InlineData(true)]
        //[InlineData(false)]
        public async Task PropertySourceName(bool useLegacy)
        {
            using (var tc = new TestContext(_output))
            {
                tc.EnvironmentName = "EnvironmentName";
                tc.ConfigureServicesForSingleTest = (ctx, services) =>
                {
                    if (useLegacy)
                    {
                        Endpoint.Env.EndpointServiceCollectionExtensions.AddEnvActuator(services, ctx.Configuration);
                        services.PostConfigure<TestOptions>(options => options.ConfigureEndpoints = endpoints => endpoints.Map<Endpoint.Env.EnvEndpoint>());
                    }
                };
                tc.ConfigureAppConfigurationForSingleTest = (ctx, configurationBuilder, values) =>
                {
                    configurationBuilder.AddJsonFile("foobar", true);

                    values.Add("management:endpoints:enabled", "true");
                    values.Add("management:endpoints:actuator:exposure:include:0", "env");
                };

                var json = await tc.HttpClient.GetStringAsync("/actuator/env").ConfigureAwait(false);

                Assert.Contains("JsonConfigurationProvider: [foobar]", json);
            }
        }

        [Theory]
        [InlineData(true)]
        //[InlineData(false)]
        public async Task PropertySourceDescriptor(bool useLegacy)
        {
            using (var tc = new TestContext(_output))
            {
                tc.EnvironmentName = "EnvironmentName";
                tc.ConfigureServicesForSingleTest = (ctx, services) =>
                {
                    if (useLegacy)
                    {
                        Endpoint.Env.EndpointServiceCollectionExtensions.AddEnvActuator(services, ctx.Configuration);
                        services.PostConfigure<TestOptions>(options => options.ConfigureEndpoints = endpoints => endpoints.Map<Endpoint.Env.EnvEndpoint>());
                    }
                };
                tc.ConfigureAppConfigurationForSingleTest = (ctx, configurationBuilder, values) =>
                {
                    values.Add("management:endpoints:enabled", "true");
                    values.Add("management:endpoints:actuator:exposure:include:0", "env");
                    values.Add("CharSize", "should not duplicate");
                    values.Add("common", "appsettings");

                    configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        { "common", "otherAppsettings" },
                        { "charSize", "should not duplicate" }
                    });
                };

                var json = await tc.HttpClient.GetStringAsync("/actuator/env").ConfigureAwait(false);

                Assert.Contains("{\"name\":\"MemoryConfigurationProvider\",\"properties\":{\"charSize\":{\"value\":\"should not duplicate\"},\"common\":{\"value\":\"otherAppsettings\"}}}", json);
                Assert.Contains("{\"name\":\"MemoryConfigurationProvider\",\"properties\":{\"CharSize\":{\"value\":\"should not duplicate\"},\"common\":{\"value\":\"appsettings\"},\"management:endpoints:actuator:exposure:include:0\":{\"value\":\"env\"},\"management:endpoints:enabled\":{\"value\":\"true\"}}}", json);
            }
        }

        //[Fact]
        //public void GetPropertySources_ReturnsExpected()
        //{
        //    var opts = new EnvEndpointOptions();
        //    var appsettings = new Dictionary<string, string>()
        //    {
        //        ["management:endpoints:enabled"] = "false",
        //        ["management:endpoints:path"] = "/cloudfoundryapplication",
        //        ["management:endpoints:loggers:enabled"] = "false",
        //        ["management:endpoints:heapdump:enabled"] = "true",
        //        ["management:endpoints:cloudfoundry:validatecertificates"] = "true",
        //        ["management:endpoints:cloudfoundry:enabled"] = "true"
        //    };
        //    var configurationBuilder = new ConfigurationBuilder();
        //    configurationBuilder.AddInMemoryCollection(appsettings);
        //    var config = configurationBuilder.Build();
        //    var env = HostingHelpers.GetHostingEnvironment();
        //    var ep = new EnvEndpoint(opts, config, env);
        //    var result = ep.GetPropertySources(config);
        //    Assert.NotNull(result);
        //    Assert.Single(result);

        //    var desc = result[0];

        //    Assert.Equal("MemoryConfigurationProvider", desc.Name);
        //    var props = desc.Properties;
        //    Assert.NotNull(props);
        //    Assert.Equal(6, props.Count);
        //    Assert.Contains("management:endpoints:cloudfoundry:validatecertificates", props.Keys);
        //    var prop = props["management:endpoints:cloudfoundry:validatecertificates"];
        //    Assert.NotNull(prop);
        //    Assert.Equal("true", prop.Value);
        //    Assert.Null(prop.Origin);
        //}

        //[Fact]
        //public void GetPropertySources_ReturnsExpected_WithPlaceholders()
        //{
        //    // arrange
        //    var appsettings = new Dictionary<string, string>()
        //    {
        //        ["management:endpoints:path"] = "/cloudfoundryapplication",
        //        ["appsManagerBase"] = "${management:endpoints:path}"
        //    };
        //    var config = new ConfigurationBuilder().AddInMemoryCollection(appsettings).AddPlaceholderResolver().Build();
        //    var endpoint = new EnvEndpoint(new EnvEndpointOptions(), config, HostingHelpers.GetHostingEnvironment());

        //    // act
        //    var result = endpoint.GetPropertySources(config);
        //    var testProp = config["appsManagerBase"];

        //    // assert
        //    Assert.NotNull(result);
        //    Assert.Equal(2, result.Count);
        //    Assert.NotNull(testProp);
        //    Assert.Equal("/cloudfoundryapplication", testProp);
        //}

        //[Fact]
        //public void DoInvoke_ReturnsExpected()
        //{
        //    var opts = new EnvEndpointOptions();
        //    var appsettings = new Dictionary<string, string>()
        //    {
        //        ["management:endpoints:enabled"] = "false",
        //        ["management:endpoints:path"] = "/cloudfoundryapplication",
        //        ["management:endpoints:loggers:enabled"] = "false",
        //        ["management:endpoints:heapdump:enabled"] = "true",
        //        ["management:endpoints:cloudfoundry:validatecertificates"] = "true",
        //        ["management:endpoints:cloudfoundry:enabled"] = "true"
        //    };
        //    var configurationBuilder = new ConfigurationBuilder();
        //    configurationBuilder.AddInMemoryCollection(appsettings);
        //    var config = configurationBuilder.Build();
        //    var env = HostingHelpers.GetHostingEnvironment();
        //    var ep = new EnvEndpoint(opts, config, env);
        //    var result = ep.DoInvoke(config);
        //    Assert.NotNull(result);
        //    Assert.Single(result.ActiveProfiles);
        //    Assert.Equal("EnvironmentName", result.ActiveProfiles[0]);
        //    Assert.Single(result.PropertySources);

        //    var desc = result.PropertySources[0];

        //    Assert.Equal("MemoryConfigurationProvider", desc.Name);
        //    var props = desc.Properties;
        //    Assert.NotNull(props);
        //    Assert.Equal(6, props.Count);
        //    Assert.Contains("management:endpoints:loggers:enabled", props.Keys);
        //    var prop = props["management:endpoints:loggers:enabled"];
        //    Assert.NotNull(prop);
        //    Assert.Equal("false", prop.Value);
        //    Assert.Null(prop.Origin);
        //}

        //[Fact]
        //public void Sanitized_ReturnsExpected()
        //{
        //    var opts = new EnvEndpointOptions();
        //    var appsettings = new Dictionary<string, string>()
        //    {
        //        ["password"] = "mysecret",
        //        ["secret"] = "mysecret",
        //        ["key"] = "mysecret",
        //        ["token"] = "mysecret",
        //        ["my_credentials"] = "mysecret",
        //        ["credentials_of"] = "mysecret",
        //        ["my_credentialsof"] = "mysecret",
        //        ["vcap_services"] = "mysecret"
        //    };
        //    var configurationBuilder = new ConfigurationBuilder();
        //    configurationBuilder.AddInMemoryCollection(appsettings);
        //    var config = configurationBuilder.Build();
        //    var env = HostingHelpers.GetHostingEnvironment();
        //    var ep = new EnvEndpoint(opts, config, env);
        //    var result = ep.DoInvoke(config);
        //    Assert.NotNull(result);

        //    var desc = result.PropertySources[0];

        //    Assert.Equal("MemoryConfigurationProvider", desc.Name);
        //    var props = desc.Properties;
        //    Assert.NotNull(props);
        //    foreach (var key in appsettings.Keys)
        //    {
        //        Assert.Contains(key, props.Keys);
        //        Assert.NotNull(props[key]);
        //        Assert.Equal("******", props[key].Value);
        //        Assert.Null(props[key].Origin);
        //    }
        //}

        //[Fact]
        //public void Sanitized_NonDefault_WhenSet()
        //{
        //    var appsettings = new Dictionary<string, string>()
        //    {
        //        ["management:endpoints:env:keystosanitize:0"] = "credentials",
        //        ["password"] = "mysecret"
        //    };

        //    var configurationBuilder = new ConfigurationBuilder();
        //    configurationBuilder.AddInMemoryCollection(appsettings);
        //    var config = configurationBuilder.Build();
        //    var opts = new EnvEndpointOptions(config);
        //    var env = HostingHelpers.GetHostingEnvironment();
        //    var ep = new EnvEndpoint(opts, config, env);
        //    var result = ep.DoInvoke(config);
        //    Assert.NotNull(result);

        //    var desc = result.PropertySources[0];
        //    Assert.Equal("MemoryConfigurationProvider", desc.Name);
        //    var props = desc.Properties;
        //    Assert.NotNull(props);
        //    Assert.Contains("password", props.Keys);
        //    Assert.NotNull(props["password"]);
        //    Assert.Equal("mysecret", props["password"].Value);
        //}
    }
}
