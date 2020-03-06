using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using HeritageWebserviceDotNetCore.Reptile;
using HeritageWebserviceReptileDotNetCore.Reptile;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HeritageWebService
{
#pragma warning disable CS1591
    public class Program
    {
        public static void Main(string[] args)
        {
            Task.Run(async ()=>
            {
                await GetIhChina.StartReptile();
            });
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Enabled = true;
            timer.Interval = TimeSpan.FromDays(1).TotalMilliseconds;
            timer.Start();
            timer.Elapsed += (o, e) =>
            {
                Task.Run(async () =>
                {
                    await GetIhChina.StartReptile();
                });
            };
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel(options=>
                {
                    options.Listen(IPAddress.Any, 5000);
                    options.Listen(IPAddress.Any, 5001, listenoptions =>
                      {
                          var password = File.ReadAllText("keystorePass.txt");
                          listenoptions.UseHttps("sunkai.xyz.pfx", password);
                      });
                });
    }
#pragma warning disable CS1591
}
