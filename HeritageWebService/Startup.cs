using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HeritageWebService.Model;
using HeritageWebServiceDotNetCore.Controllers;
using HeritageWebServiceDotNetCore.Model;
using HeritageWebServiceDotNetCore.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

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
            services.Configure<HeritageMongodbSettings>(Configuration.GetSection(nameof(HeritageMongodbSettings)));//将appsetings.json当中的属性进行依赖填充
            services.AddSingleton<IHeritageMongodbSettings>(sp => sp.GetRequiredService<IOptions<HeritageMongodbSettings>>().Value); //接口的单一实例以单例在服务生存期DI中注册
            //向DI注册了HeritageService的类，以支持消费类中的构造函数注入。单例存在于整个服务周期是最合适的。 根据官方 Mongo Client 重用准则，应使用单一实例服务生存期在 DI 中注册 MongoClient。
            services.AddSingleton<BannerService>(); 
            services.AddSingleton<NewsDetailService>();
            services.AddSingleton<NewsListService>();
            services.AddSingleton<HeritageProjectServicecs>();

            //将Swagger生成器添加到方法中的服务集合中
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {
                    Version = "v1",
                    Title = "APIs",
                    Description = "A Heritage WebService ASP.NET Core Web API",
                    Contact = new OpenApiContact
                    {
                        Name = "Kai Sun",
                        Email = string.Empty,
                        Url = new Uri("https://github.com/sunkaiiii"),
                    }
                });
                //注册文件中的xml信息给Swagger
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            //为所有的请求添加拦截器
            services.AddMvc(options => {
                options.Filters.Add<BaseInformationFilter>();
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseStaticFiles();
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "img");
            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "img")),
                RequestPath = "/img"
            });
            //允许中间件为生成的JSON文档和Swagger UI提供服务
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = string.Empty; //在应用的根提供Swagger UI。将前缀设置为空
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints=>endpoints.MapControllers());
        }
    }
}
