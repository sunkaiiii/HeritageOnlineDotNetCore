using HeritageWebService.Model;
using HeritageWebService.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebServiceDotNetCore.Controllers
{
    //使用HeritageService进行CRUD操作。
    //包含操作方法以支持GET/POST/PUT/DELETE HTTP请求。
    //Create操作方法中调用CreatedAtRoute以返回HTTP 201象映。状态码201是在服务器上创建新资源的HTTP POST方法的标准相应。
    //将 [Produces("application/json")] 属性添加到 API 控制器以声明控制器的操作支持 application/json 的响应内容类型 

    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class HeritageController : ControllerBase
    {
        private readonly HeritageService _heritageService;

        public HeritageController(HeritageService heritageService)
        {
            _heritageService = heritageService;
        }

        [HttpGet]
        public ActionResult<List<MainNewsList>> Get() => _heritageService.Get();

        /// <response code="200">Returns the specific news</response>
        /// <response code="400">If the news is null</response>  
        [HttpGet("{id:length(24)}",Name ="GetMainNews")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<MainNewsList> Get(string id)
        {
            var news = _heritageService.Get(id);
            if(news == null)
            {
                return NotFound();
            }
            return news;
        }

        /// <summary>
        /// Create a new news
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///      POST /Heritage
        ///     {
        ///        "detailPageUrl": "/news/breakingnews/asd.html,
        ///        "title": "萨达费瓦分为氛围 ",
        ///        "date": 2019.03.03"
        ///     }
        /// </remarks>
        [HttpPost]
        public ActionResult<MainNewsList> Create(MainNewsList news)
        {
            _heritageService.Create(news);
            return CreatedAtRoute("GetMainNews", new { id = news.Id.ToString() }, news);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, MainNewsList newsIn)
        {
            var news = _heritageService.Get(id);
            if(news==null)
            {
                return NotFound();
            }
            _heritageService.Update(id, newsIn);
            return NoContent();
        }

        /// <summary>
        /// Deletes a specific News
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var news = _heritageService.Get(id);
            if(news == null)
            {
                return NotFound();
            }
            _heritageService.Remove(news.Id);
            return NoContent();
        }
    }
}
