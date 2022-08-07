using LoginandRegisterMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Filters;

namespace LoginandRegisterMVC.Controllers
{
    public class AllowCrossSiteJsonAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Response != null)
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            base.OnActionExecuted(actionExecutedContext);
        }
    }

    //[AllowCrossSiteJson]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserReactController : ApiController
    {
         // GET api/<controller>
         private UserContext db = new UserContext();

        [HttpGet]
        [ActionName("Get")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet]
        [ActionName("Get/{id}")]
        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet]
       [ActionName("GetUsers")]
        public IHttpActionResult GetUsers()
        {

            var entity = db.Users.ToList();
            db.Database.CommandTimeout = 300;
                if (entity != null)
                {
                return Ok(entity);
                }
                else
                {
                return Ok("Invalid");
                }
            
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}