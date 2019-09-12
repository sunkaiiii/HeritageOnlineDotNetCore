using HeritageWebServiceDotNetCore.Model;
using HeritageWebServiceDotNetCore.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebServiceDotNetCore.Controllers
{
    //[Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class HeritageProjectController : ControllerBase
    {
        private HeritageProjectServicecs _heritageProjectService;
        public HeritageProjectController(HeritageProjectServicecs service)
        {
            _heritageProjectService = service;
        }

        ///<summary>
        ///get the information of the main page of the heritage project
        ///</summary>
        [HttpGet]
        [Route("/api/[controller]/GetMainPage")]
        [ProducesResponseType(200)]
        public ActionResult<HeritageProjectMainPage> Get()
        {
            return _heritageProjectService.GetMainPage();
        }


        /// <summary>
        /// get a list of heritage projects
        /// </summary>
        /// <param name="pages">which page should be displayed</param>
        [Route("/api/[controller]/GetHeritageProjectList/{pages}")]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public ActionResult<List<HeritageProject>> GetHeritageProjectList(string pages)
        {
            var isNumric = int.TryParse(pages, out int n);
            if (isNumric)
            {
                return GetHeritageProjectList(n);
            }
            return BadRequest();
        }

        private ActionResult<List<HeritageProject>> GetHeritageProjectList(int pages)
        {
            var list = _heritageProjectService.GetProjectList(pages);
            if (list == null)
            {
                return NotFound();
            }
            return list;
        }

        /// <summary>
        /// return the detail for a specific heritage project
        /// </summary>
        /// <param name="link"></param>
        /// <returns>a detail model</returns>
        [Route("/api/[controller]/GetHeritageDetail")]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<HeritageProjectDetail> GetHeritageDetal([FromQuery(Name = "link")]string link)
        {
            var news = _heritageProjectService.GetProjectDetail(link);
            if (news == null)
            {
                return NotFound();
            }
            return news;
        }

        /// <summary>
        /// give a list of heritage projects which meet  the filter condition
        /// </summary>
        /// <param name="num">number of the project</param>
        /// <param name="title">title name of the project</param>
        /// <param name="type">the type of the project</param>
        /// <param name="rx_time">the time of this project been produced</param>
        /// <param name="cate">the category of the project</param>
        /// <param name="province">the location of the project</param>
        /// <param name="unit">the unit location of the project</param>
        /// <param name="pages">the page number of the project</param>
        /// <returns></returns>
        [Route("/api/[controller]/SearchHeritageProject")]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<List<HeritageProject>> SearchHeritageProject([FromQuery(Name = "num")]string num
            , [FromQuery(Name = "title")]string title
            , [FromQuery(Name = "type")]string type
            , [FromQuery(Name = "rx_time")]string rx_time
            , [FromQuery(Name = "cate")]string cate
            , [FromQuery(Name = "province")]string province
            , [FromQuery(Name = "unit")]string unit
            ,[FromQuery(Name ="page")]int pages=1
            )
        {
            HeritageProject filter = new HeritageProject();
            filter.num = num;
            filter.title = title;
            filter.rx_time = rx_time;
            filter.cate = cate;
            filter.province = province;
            filter.unit = unit;
            return _heritageProjectService.GetFilterSearchProjectList(filter, pages);
        }

        [Route("/api/[controller]/GetSearchCategories")]
        [HttpGet]
        [ProducesResponseType(200)]
        public string  GetSearchCategories()
        {
            var dic = _heritageProjectService.GetAllCategories();
            return JsonConvert.SerializeObject(_heritageProjectService.GetAllCategories());
        }

    }
}
