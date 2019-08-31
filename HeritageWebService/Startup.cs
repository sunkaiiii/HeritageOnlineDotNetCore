using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeritageWebService.Model;
using HeritageWebService.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HeritageWebService
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
            services.Configure<HeritageMainPageListSettings>(Configuration.GetSection(nameof(HeritageMainPageListSettings))); //将appsetings.json当中的属性进行依赖填充
            services.AddSingleton<IHeritageMainPageListSettings>(sp => sp.GetRequiredService<IOptions<HeritageMainPageListSettings>>().Value); //接口的单一实例以单例在服务生存期DI中注册
            services.AddSingleton<HeritageService>(); //向DI注册了HeritageService的类，以支持消费类中的构造函数注入。单例存在于整个服务周期是最合适的。 根据官方 Mongo Client 重用准则，应使用单一实例服务生存期在 DI 中注册 MongoClient。
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
