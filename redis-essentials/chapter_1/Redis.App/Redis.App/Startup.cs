using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;
using Redis.Libs.Queue;
using StackExchange.Redis;

namespace Redis.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var redisClientConnection = ConnectionMultiplexer.Connect("localhost");
            services.AddSingleton<IConnectionMultiplexer>(redisClientConnection);
            services.AddSingleton<IDatabaseAsync>(provider =>
            {
                var connectionMultiplexer = provider.GetService<IConnectionMultiplexer>();
                return connectionMultiplexer.GetDatabase(0);
            });
            services.AddSingleton<IQueue, Queue>();
            services.AddControllers();
            services.AddOpenTelemetryTracing(
                (builder) =>
                {
                    builder
                        .AddAspNetCoreInstrumentation()
                        .AddRedisInstrumentation(redisClientConnection)
                        .AddJaegerExporter();
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
