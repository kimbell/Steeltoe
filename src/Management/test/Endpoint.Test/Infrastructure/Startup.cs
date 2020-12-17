// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Steeltoe.Management.Endpoint.Test.Infrastructure
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<TestOptions>(Configuration.GetSection(TestOptions.SectionName));
            services.AddRouting();
        }

        public void Configure(IApplicationBuilder app)
        {
            var testOptions = app.ApplicationServices.GetRequiredService<IOptions<TestOptions>>().Value;

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                testOptions.ConfigureEndpoints?.Invoke(endpoints);
            });
        }
    }
}
