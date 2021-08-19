using ExploreCalifornia.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace ExploreCalifornia.Controllers
{
    public class AuthorizeController : ApiController
    {
        public IHttpActionResult Post(AuthorizeRequestDto requestDto)
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
    }
}
