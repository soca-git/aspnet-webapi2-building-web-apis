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
        public List<Tour> Get(bool freeOnly)
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
        public List<Tour> PostSearch(TourSearchRequestDto request)
        {
            IQueryable<Tour> query = _exploreCaliforniaDbContext.Tours.AsQueryable()
                .Where(x => x.Price >= request.MinPrice && x.Price <= request.MaxPrice);
            // Note; Web API will automatically serialize the list object into JSON.
            return query.ToList();
        }

        // PUT /api/tour
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