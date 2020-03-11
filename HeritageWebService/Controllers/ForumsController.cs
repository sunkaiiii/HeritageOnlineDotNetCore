using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeritageWebServiceDotNetCore.Model;
using HeritageWebServiceDotNetCore.Service;
using Microsoft.AspNetCore.Mvc;

namespace HeritageWebServiceDotNetCore.Controllers
{
    [Produces("application/json")]
    [ApiController]
    public class ForumsController : ControllerBase
    {
        private ForumsService _forumsService;
        public ForumsController(ForumsService service)
        {
            _forumsService = service;
        }

        /// <summary>
        /// get the information of the forums page in a list structure
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/ForumsList/{pages}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public ActionResult<List<NewsList>> GetForumsList(string pages)
        {
            var isNumric = int.TryParse(pages, out int n);
            if(isNumric)
            {
                return GetForumsList(n);
            }
            return BadRequest();
        }

        private ActionResult<List<NewsList>> GetForumsList(int pages)
        {
            var list = _forumsService.GetForumsList(pages);
            if(list==null||list.Count==0)
            {
                return NotFound();
            }
            return list;
        }

        /// <summary>
        /// Get the information of the forums with the link as the query string
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        /// 
        [Route("/api/[controller]/GetForumsDetail")]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<NewsDetail> GetForumsDetail([FromQuery(Name = "link")]string link)
        {
            if(String.IsNullOrEmpty(link))
            {
                return NotFound();
            }
            var forumsDetail = _forumsService.GetForumsDetail(link);
            if(forumsDetail==null)
            {
                return NotFound();
            }
            return forumsDetail;
        }

    }
}