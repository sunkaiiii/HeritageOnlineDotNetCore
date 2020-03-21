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
    public class PeopleController:ControllerBase
    {
        private PeoplePageService _peopleService;

        public PeopleController(PeoplePageService service)
        {
            _peopleService = service;
        }

        /// <summary>
        /// get the information of the people main page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/GetPeopleMainPage")]
        [ProducesResponseType(200)]
        [ProducesResponseType(200)]
        public PeoplePage.PeopleMainPage GetPeopleMainpage()
        {
            var mainPageInfo = _peopleService.GetPeopleMainPage();
            return mainPageInfo;
        }

        [HttpGet]
        [Route("api/[controller]/PeopleList/{page}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<List<NewsList>> GetPeopleList(string page)
        {
            var isNumric = int.TryParse(page, out int n);
            if (isNumric)
            {
                var list = _peopleService.GetPeopleList(n);
                if (list == null || list.Count == 0)
                {
                    return NotFound();
                }
                return list;
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("api/[controller]/GetPeopleDetail")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<NewsDetail> GetPeopleDetail([FromQuery(Name = "link")]string link)
        {
            if (string.IsNullOrEmpty(link))
                return NotFound();
            var result = _peopleService.GetPeopleDetail(link);
            if (result == null)
                return NotFound();
            return result;
        }
    }
}
