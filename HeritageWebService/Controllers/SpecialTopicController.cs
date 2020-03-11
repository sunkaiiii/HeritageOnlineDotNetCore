using HeritageWebServiceDotNetCore.Model;
using HeritageWebServiceDotNetCore.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebServiceDotNetCore.Controllers
{
    [Produces("application/json")]
    [ApiController]
    public class SpecialTopicController:ControllerBase
    {
        private SpecialTopicService _specialTopicService;
        public SpecialTopicController(SpecialTopicService service)
        {
            _specialTopicService = service;
        }


        /// <summary>
        /// get a list of inforamtion of special topics
        /// </summary>
        /// <param name="pages"></param>
        /// <returns></returns>
        [Route("api/[controller]/GetSpecialTopicList/{pages}")]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<List<NewsList>> GetSpecialTopicList(string pages)
        {
            var isNummric = int.TryParse(pages, out int n);
            if(isNummric)
            {
                return GetSpecialTopicList(n);
            }
            return BadRequest();
        }

        private ActionResult<List<NewsList>> GetSpecialTopicList(int page)
        {
            var resultList = _specialTopicService.GetSpecialTopicList(page);
            if(resultList==null)
            {
                return NotFound();
            }
            return resultList;
        }

        /// <summary>
        /// get a specific information of special detail by link
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        [Route("api/[controller]/GetSpecialTopicDetail")]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<NewsDetail> GetSpecialTopicDetail([FromQuery(Name ="link")]string link)
        {
            var news = _specialTopicService.GetSpecialTopicDetail(link);
            if(news==null)
            {
                return NotFound();
            }
            return news;
        }
    }
}
