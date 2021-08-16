using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ExploreCalifornia.DataAccess;
using ExploreCalifornia.DTOs;

namespace ExploreCalifornia.Controllers
{
    public class TourController : ApiController
    {
        public IHttpActionResult Get()
        {
            return Ok("Get");
        }

        public IHttpActionResult Post()
        {
            return Ok("Post");
        }
        
        public IHttpActionResult Put()
        {
            return Ok("Put");
        }

        public IHttpActionResult Patch()
        {
            return Ok("Patch");
        }
        public IHttpActionResult Delete()
        {
            return Ok("Delete");
        }
    }
}