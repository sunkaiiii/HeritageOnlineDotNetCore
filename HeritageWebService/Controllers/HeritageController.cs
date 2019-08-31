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
    
    [Route("api/[controller]")]
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

        [HttpGet("{id:length(24)}",Name ="GetMainNews")]
        public ActionResult<MainNewsList> Get(string id)
        {
            var news = _heritageService.Get(id);
            if(news == null)
            {
                return NotFound();
            }
            return news;
        }

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
