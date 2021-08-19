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
    // Common route prefix for all routes in this controller.
    [RoutePrefix("api/v2/tour")]
    public class TourController : ApiController
    {
        private ExploreCaliforniaDbContext _exploreCaliforniaDbContext = new ExploreCaliforniaDbContext();

        // GET /api/v2/tour
        //public List<Tour> GetTours()
        public List<TourDto> GetTours()
        {
            // Since we are using a DTO, we map the database query to the DTO inside the Select statement.
            IQueryable<TourDto> query = _exploreCaliforniaDbContext.Tours.AsQueryable()
                .Select(
                    x => new TourDto
                    {
                        // TourDTO -> x.Tour
                        Name = x.Name,
                        Description = x.Description,
                        Price = x.Price
                    }
                );

            // Note; Web API will automatically serialize the list object into JSON.
            return query.ToList();
            // Returns string "Get" in reponse body.
            // return Ok("Post"); (IHttpActionResult type)
        }

        // GET /api/v2/tour?freeOnly=true
        // Simple data type parameters are bound from URI through Web API binding conventions.
        // These default binding conventions can be overridden/explicitly specified through [FromUri] and [FromBody] attributes.
        [HttpGet]
        public List<Tour> GetTours([FromUri] bool freeOnly)
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

        // GET /api/v2/tour/<id>
        // Route attribute includes a constraint on the id component of the URI,
        // meaning it must be of type integer.
        [HttpGet]
        [Route("{id:int}")]
        public Tour GetById(int id)
        {
            Tour tour = _exploreCaliforniaDbContext.Tours.AsQueryable()
                .Where(x => x.TourId == id)
                .FirstOrDefault();

            // Note; Web API will automatically serialize the list object into JSON.
            return tour;
        }

        // GET /api/v2/tour/<name>
        // This won't work using the default rule because {id} can be of any type.
        // This means the request will actually go to the method above, not this one,
        // since we don't differentiate the type of  {id} in the routing rule.
        // This can be resolved by specifying a constraint on the rule and by adding an additional one for strings.
        [HttpGet]
        [Route("{name}")]
        public Tour GetByName(string name)
        {
            Tour tour = _exploreCaliforniaDbContext.Tours.AsQueryable()
                .Where(x => x.Name.Contains(name))
                .FirstOrDefault();

            // Note; Web API will automatically serialize the list object into JSON.
            return tour;
        }

        // POST /api/v2/tour
        // Parameter is bound from Body through Web API binding conventions.
        // Web API binds non-simple types from the body by default.
        // These default binding conventions can be explicitly specified/overridden through [FromUri] and [FromBody] attributes.
        // The method name PostSearch was required to start with 'Post' in order for it to bind to a POST request.
        // This default naming convention can be ignored by using Web API HTTP verb attributes.
        // These verb attributes allow explicit definition of what HTTP verb this method should bind to,
        [HttpPost]
        public IHttpActionResult SearchTours([FromBody] TourSearchRequestDto requestDto)
        {
            // Handling exceptions can be done in the controller, but for larger applications with increasing amounts of 
            // checks, the controllers would become bloated very quickly. Exception filters can be used to resolve this issue.
            if (requestDto.MinPrice < 0 || requestDto.MaxPrice < 0)
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

        // POST /api/v2/tour
        // HttpResponseExceptions in combination with the strongly typed return method (List<Tour>) can also be used (and are!).
        // This achieves the same results as the previous method which uses IHttpActionResult.
        // The potential drawbacks here are exceptions are being used to handle the flow or the application,
        // which is generally not seen as good practice. It can also complicate logging and global exception handling.
        // '~' escapes the controller's route prefix.
        [HttpPost]
        [Route("~/api/v1/tour")]
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

        // PUT /api/v2/tour/<id>
        // Id parameter is bound from Route attribute.
        // Tour parameter is automatically bound from the response body.
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateTour(int id, Tour tour)
        {
            return Ok($"Received: {id}; {tour.Name}");
        }

        // PATCH /api/v2/tour
        [HttpPatch]
        public IHttpActionResult Patch()
        {
            return Ok("Patch");
        }

        // DELETE /api/v2/tour/<id>
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            return Ok("Delete");
        }
    }
}
