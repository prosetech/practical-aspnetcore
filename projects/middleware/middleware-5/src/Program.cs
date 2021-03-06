using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Hosting;

namespace PracticalAspNetCore
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.Map("/hello", (IApplicationBuilder pp) => {
                //nested
                app.Map("/world", (IApplicationBuilder ppa) => ppa.Run(context => 
                    context.Response.WriteAsync($"Path: {context.Request.Path} - Path Base: {context.Request.PathBase}")));
                
                pp.Run(context => 
                    context.Response.WriteAsync($"Path: {context.Request.Path} - Path Base: {context.Request.PathBase}"));
            });
            
            app.Run(context =>
            { 
                context.Response.Headers.Add("Content-Type", "text/html");
                return context.Response.WriteAsync(@"
                   <a href=""/hello"">/hello</a> <a href=""/hello/world"">/hello/world</a>
                ");
            });
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder.UseStartup<Startup>()
                );
    }
}