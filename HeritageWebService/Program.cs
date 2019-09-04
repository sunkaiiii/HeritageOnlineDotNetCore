using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HeritageWebserviceDotNetCore.Reptile;
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
            ThreadPool.QueueUserWorkItem((state)=>GetIhChina.StartReptile());
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
#pragma warning disable CS1591
}
