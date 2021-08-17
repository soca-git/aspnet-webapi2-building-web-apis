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

        // GET /api/tour
        public List<Tour> Get()
        {
            IQueryable<Tour> query = _exploreCaliforniaDbContext.Tours.AsQueryable();
            // Note; Web API will automatically serialize the list object into JSON.
            return query.ToList();
            // Returns string "Get" in reponse body.
            // return Ok("Post"); (IHttpActionResult type)
        }

        // GET /api/tour?freeOnly=true
        // Simple data type parameters are bound from URI through Web API binding conventions.
        // These default binding conventions can be overridden/explicitly specified through [FromUri] and [FromBody] attributes.
        [HttpGet]
        public List<Tour> Get([FromUri]bool freeOnly)
        {
            IQueryable<Tour> query = _exploreCaliforniaDbContext.Tours.AsQueryable();
            if (freeOnly)
            {
                // 0.0m -> decimal type.
                query = query.Where(x => x.Price == 0.0m);
            }
            // Note; Web API will automatically serialize the list object into JSON.
            return query.ToList();
        }

        // POST /api/tour
        // Parameter is bound from Body through Web API binding conventions.
        // Web API binds non-simple types from the body by default.
        // These default binding conventions can be explicitly specified/overridden through [FromUri] and [FromBody] attributes.
        // The method name PostSearch was required to start with 'Post' in order for it to bind to a POST request.
        // This default naming convention can be ignored by using Web API HTTP verb attributes.
        // These verb attributes allow explicit definition of what HTTP verb this method should bind to,
        [HttpPost]
        public List<Tour> SearchTours([FromBody]TourSearchRequestDto request)
        {
            IQueryable<Tour> query = _exploreCaliforniaDbContext.Tours.AsQueryable()
                .Where(x => x.Price >= request.MinPrice && x.Price <= request.MaxPrice);
            // Note; Web API will automatically serialize the list object into JSON.
            return query.ToList();
        }

        // PUT /api/tour/1
        // Id parameter is bound from URI.
        // Tour parameter is bound from the body.
        [HttpPut]
        public IHttpActionResult Put(int id, Tour tour)
        {
            return Ok($"Received: {id}; {tour.Name}");
        }

        // PATCH /api/tour
        public IHttpActionResult Patch()
        {
            return Ok("Patch");
        }

        // DELETE /api/tour
        public IHttpActionResult Delete()
        {
            return Ok("Delete");
        }
    }
}