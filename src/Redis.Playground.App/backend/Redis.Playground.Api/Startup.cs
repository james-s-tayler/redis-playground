using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Trace;
using Prometheus;
using Prometheus.DotNetRuntime;
using Prometheus.SystemMetrics;
using Redis.Playground.Libs.Queue;
using StackExchange.Redis;

namespace Redis.Playground.Api
{
    public class Startup
    {
        private static IDisposable _collector;
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _collector = DotNetRuntimeStatsBuilder.Default().StartCollecting();
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
                builder =>
                {
                    builder
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddSqlClientInstrumentation()
                        .AddRedisInstrumentation(redisClientConnection)
                        .AddJaegerExporter();
                });
            
            services.AddHttpClient("InstrumentedHttpClient")
                    .UseHttpClientMetrics();
            services.AddHttpContextAccessor();
            services.AddSystemMetrics();
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
            app.UseHttpMetrics();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapMetrics();
            });
        }
    }
}