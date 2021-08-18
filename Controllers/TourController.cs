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
        public IHttpActionResult SearchTours([FromBody]TourSearchRequestDto requestDto)
        {
            if (requestDto.MinPrice < 0 ||  requestDto.MaxPrice < 0)
            {
                // Returns 400 response with specified message in response body.
                return BadRequest("Specified price limits must be greater than 0.");
            }

            if (requestDto.MinPrice > requestDto.MaxPrice)
            {
                // Returns 400 response with specified message in response body.
                return BadRequest("MinPrice must be less than MaxPrice.");
            }

            IQueryable<Tour> query = _exploreCaliforniaDbContext.Tours.AsQueryable()
                .Where(x => x.Price >= requestDto.MinPrice && x.Price <= requestDto.MaxPrice);

            // Returns 200 response with JSON object containing query result(s).
            // Note; Web API will automatically serialize the list object into JSON.
            return Ok(query.ToList());
            // Returned objects can be wrapped into an IHttpActionResult returning helper method.
            // This allows the flexibility of allowing other http helper methods to be used to return other responses.
            // (We are no longer restricted to returning List<Tour> only).
        }

        [HttpPost]
        [Route("api/v2/tour")]
        // HttpResponseExceptions in combination with the strongly typed return method (List<Tour>) can also be used (and are!).
        // This achieves the same results as the previous method which uses IHttpActionResult.
        // The potential drawbacks here are exceptions are being used to handle the flow or the application,
        // which is generally not seen as good practice. It can also complicate logging and global exception handling.
        public List<Tour> SearchToursWithExceptions([FromBody] TourSearchRequestDto requestDto)
        {
            if (requestDto.MinPrice < 0 || requestDto.MaxPrice < 0)
            {
                // Returns 400 response with specified message in response body.
                // In order for HttpResponseException to run during debugging, it needs to be added as an exception to
                // CLR Exceptions (Windows -> Exception Settings).
                // Once added, right-click and select 'Continue when unhandled in user code'.
                // Also deselect 'Break When Thrown' checkbox.
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("Specified price limits must be greater than 0.")
                });
            }

            if (requestDto.MinPrice > requestDto.MaxPrice)
            {
                // Returns 400 response with specified message in response body.
                throw new HttpResponseException(
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Content = new StringContent("MinPrice must be less than MaxPrice.")
                    }
                );
            }

            IQueryable<Tour> query = _exploreCaliforniaDbContext.Tours.AsQueryable()
                .Where(x => x.Price >= requestDto.MinPrice && x.Price <= requestDto.MaxPrice);

            // Returns 200 response with JSON object containing query result(s).
            // Note; Web API will automatically serialize the list object into JSON.
            return query.ToList();
            // Returned objects can be wrapped into an IHttpActionResult returning helper method.
            // This allows the flexibility of allowing other http helper methods to be used to return other responses.
            // (We are no longer restricted to returning List<Tour> only).
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