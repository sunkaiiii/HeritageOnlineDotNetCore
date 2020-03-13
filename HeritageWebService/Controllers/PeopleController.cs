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
    }
}
