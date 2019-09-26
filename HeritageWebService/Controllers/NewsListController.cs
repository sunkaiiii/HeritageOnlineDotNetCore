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
    public class NewsListController:ControllerBase
    {
        private readonly NewsListService _newsListService;

        public NewsListController(NewsListService service)
        {
            _newsListService = service;
        }

        /// <summary>
        /// return basic newslist,from index 1 to index 20
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<List<NewsList>> Get()
        {
            return GetList(1);
        }

        /// <summary>
        /// return news list based on the page number
        /// </summary>
        /// <param name="pages"></param>
        /// <returns></returns>
        /// <response code="200">Returns the specific news list</response>
        /// <response code="400">return if pages number is incorrect or the page number is out of the bound</response>  
        [HttpGet("{pages}", Name = "GetNewsList")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<List<NewsList>> Get(string pages)
        {
            var isNumric = int.TryParse(pages, out int n);
            if (isNumric)
            {
                return GetList(n);
            }
            return BadRequest();
        }

        private ActionResult<List<NewsList>> GetList(int pages)
        {
            var list = _newsListService.Get(pages);
            if (list == null)
            {
                return NotFound();
            }
            return list;
        }
    }
}
