using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sunday.Core.Media.Application;
using Sunday.Core.Media.Extensions;
using Sunday.Core.Media.Implementation;
using Sunday.MediaServer.Models;

namespace Sunday.MediaServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApiKeyConfiguration>(Configuration.GetSection("ApiKeys"));
            var storagePath = Configuration.GetValue<string>("StoragePath");
            var fileBlobProvider = new FileBlobProvider(Environment, storagePath, "false");
            services.AddSingleton<IBlobProvider>(fileBlobProvider);
            services.AddSingleton<IApplicationMediaManager>(
                new DefaultApplicationMediaManager(fileBlobProvider, new DefaultBlobLinkManager()));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
            app.UseBlobPreview();
        }
    }
}
