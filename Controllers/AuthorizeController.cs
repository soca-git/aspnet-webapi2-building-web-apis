using ExploreCalifornia.Data;
using ExploreCalifornia.DTOs;
using ExploreCalifornia.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace ExploreCalifornia.Controllers
{
    public class AuthorizeController : ApiController
    {
        private ExploreCaliforniaDbContext _exploreCaliforniaDbContext = new ExploreCaliforniaDbContext();
        private readonly JwtTokenHelper _tokenHelper = new JwtTokenHelper();


        [HttpPost]
        [Route("api/v2/validation")]
        // DTO (model) validation attributes test endpoint.
        public IHttpActionResult TestDtoValidation(AuthorizeRequestDto requestDto)
        {
            // Since the DTO includes validation attributes,
            // we can use ModelState.IsValid() to check them all.
            if (!ModelState.IsValid)
            {
                // Returns:
                // {
                //    "Message": "The request is invalid.",
                //    "ModelState": {
                //        "requestDto.AppToken": [
                //            "The AppToken field is required.",
                //            "The field AppToken must be a string or array type with a minimum length of '32'."
                //        ],
                //        "requestDto.AppSecret": [
                //            "The AppSecret field is required.",
                //            "The field AppSecret must be a string or array type with a minimum length of '32'."
                //        ]
                //    }
                // }
                return BadRequest(ModelState);
            }
            return Ok();
        }

        // Get a list of valid application ids which will be able to receive a JWT.
        [HttpGet]
        public List<AuthorizedClientDto> GetAuthorizedApps()
        {
            return _exploreCaliforniaDbContext.AuthorizedApps
                .Select(x => new AuthorizedClientDto
                {
                    Name = x.Name,
                    TokenExpiration = x.TokenExpiration
                }).ToList();
        }

        // Unsecured endpoint to receive JWT tokens based on data posted in request body;
        // AuthorizedClientDto (AppToken, AppSecret).
        // Checks existence of client's credentials within authorization database table.
        public IHttpActionResult Post(AuthorizeRequestDto request)
        {
            // If the DTO data is not in the expected format.
            if (!ModelState.IsValid)
            {
                // Return a 400 bad request response detailing why.
                return BadRequest(ModelState);
            }

            var authorizedClient = _exploreCaliforniaDbContext.AuthorizedApps
                .FirstOrDefault(i => 
                    i.AppToken == request.AppToken && i.AppSecret == request.AppSecret
                    //&& DateTime.UtcNow < i.TokenExpiration, commented since tokens in database are now expired.
                );

            // If the client's credentials within the DTO do not match any database entry.
            if (authorizedClient == null)
            {
                // This client should not be allowed access to the secured service,
                // do not generate an access token. Return a 401 unauthorized response.
                return Unauthorized();
            }

            // Since credentials are found within the authorization database table,
            // generate a new access token and return it to the client.
            var token = _tokenHelper.CreateToken(authorizedClient);
            return Ok(token);
        }
    }
}
