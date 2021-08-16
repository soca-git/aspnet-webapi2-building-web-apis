using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ExploreCalifornia.Data;
using ExploreCalifornia.Data.Entities;
using ExploreCalifornia.DTOs;

namespace ExploreCalifornia.Controllers
{
    public class TourController : ApiController
    {
        private ExploreCaliforniaDbContext _exploreCaliforniaDbContext = new ExploreCaliforniaDbContext();

        // GET /api/order
        public List<Tour> Get()
        {
            var query = _exploreCaliforniaDbContext.Tours.AsQueryable();

            // Note:
            // Web API will automatically serialize the list object into JSON.
            return query.ToList();
            // Returns string "Get" in reponse body.
            // return Ok("Post"); (IHttpActionResult type)
        }

        // POST /api/order
        public IHttpActionResult Post()
        {
            return Ok("Post");
        }

        // PUT /api/order
        public IHttpActionResult Put()
        {
            return Ok("Put");
        }

        // PATCH /api/order
        public IHttpActionResult Patch()
        {
            return Ok("Patch");
        }

        // DELETE /api/order
        public IHttpActionResult Delete()
        {
            return Ok("Delete");
        }
    }
}