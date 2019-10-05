using HeritageWebServiceDotNetCore.Model;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebServiceDotNetCore.Controllers
{
    public class BaseInformationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var query = context.HttpContext.Request.Query;
            if(query.ContainsKey(typeof(BaseRequest).Name))
            {
                var baseRequestString = query[typeof(BaseRequest).Name];
                var baseSetting=JsonConvert.DeserializeObject<BaseRequest>(baseRequestString);
                Console.WriteLine("baseSetting!!!!!!!!!!!"+baseSetting.ToString());
            }
        }
    }
}
