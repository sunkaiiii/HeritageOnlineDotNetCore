using HeritageWebServiceDotNetCore.Model;
using HeritageWebServiceDotNetCore.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebServiceDotNetCore.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class NewsDetailController : ControllerBase
    {
        private readonly NewsDetailService _newsDetailService;

        public NewsDetailController(NewsDetailService newsDetailService)
        {
            _newsDetailService = newsDetailService;
        }

        /// <summary>
        /// response a specific news detail content
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns the specific news detail</response>
        /// <response code="400">If the news is null</response>  
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<NewsDetail> Get([FromQuery(Name = "link")]string link)
        {
            var news = _newsDetailService.Get(link);
            if (news == null)
            {
                return NotFound();
            }
            return news;
        }
    }
}
