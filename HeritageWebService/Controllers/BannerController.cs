using HeritageWebService.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeritageWebServiceDotNetCore.Service;

namespace HeritageWebServiceDotNetCore.Controllers
{
    //使用HeritageService进行CRUD操作。
    //包含操作方法以支持GET/POST/PUT/DELETE HTTP请求。
    //Create操作方法中调用CreatedAtRoute以返回HTTP 201象映。状态码201是在服务器上创建新资源的HTTP POST方法的标准相应。
    //将 [Produces("application/json")] 属性添加到 API 控制器以声明控制器的操作支持 application/json 的响应内容类型 

    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        private readonly BannerService _bannerService;

        public BannerController(BannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [HttpGet]
        public ActionResult<List<Banner>> Get() => _bannerService.Get();

        /// <response code="200">Returns the specific news</response>
        /// <response code="400">If the news is null</response>  
        [HttpGet("{id:length(24)}",Name ="GetMainNews")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<Banner> Get(string id)
        {
            var news = _bannerService.Get(id);
            if(news == null)
            {
                return NotFound();
            }
            return news;
        }
        
    }
}
