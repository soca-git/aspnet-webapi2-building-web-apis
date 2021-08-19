using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ExploreCalifornia.Data;
using ExploreCalifornia.Data.Entities;
using ExploreCalifornia.Filters;


// GENERATED CONTROLLER!
// Controller generated using Add -> Controller -> Web API 2 Controller with actions, using Entity Framework.
// To use Entity Framework we specify the ExploreCaliforniaDbContext and Reservation data entity.
namespace ExploreCalifornia.Controllers
{
    // Exception filter attributes can be added to the controller in order for it to apply to all controller methods.
    // [DbUpdateExceptionFilter]
    public class ReservationController : ApiController
    {
        private ExploreCaliforniaDbContext db = new ExploreCaliforniaDbContext();

        // GET: api/Reservation
        public IQueryable<Reservation> GetReservations()
        {
            return db.Reservations.Include(x => x.Tour);
        }

        // GET: api/Reservation/5
        [ResponseType(typeof(Reservation))]
        public IHttpActionResult GetReservation(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return NotFound();
            }

            return Ok(reservation);
        }

        // PUT: api/Reservation/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutReservation(int id, Reservation reservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reservation.ReservationId)
            {
                return BadRequest();
            }

            db.Entry(reservation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Reservation
        [DbUpdateExceptionFilter]
        [ResponseType(typeof(Reservation))]
        public IHttpActionResult PostReservation(Reservation reservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Reservations.Add(reservation);
            db.SaveChanges();

            // Wrap save changes in a try catch statement.
            // Now an attempt to save a reservation with an existing id (which will throw an SQL exception)
            // can be handled appropriately, with a suitable HttpResponseException,
            //try
            //{
            //    db.SaveChanges();
            //}
            //catch (DbUpdateException dbUpdateException)
            //{
            //    // If the inner exception is not an SQL exception.
            //    if (!(dbUpdateException?.InnerException is SqlException sqlException))
            //    {
            //        // Re-raise the exception caught originally (dbUpdateException).
            //        throw;
            //    }

            //    // SQL Server docs: Unique constraint violation.
            //    if (sqlException.Number == 2627)
            //    {
            //        // Throw an appropriate HttpResponseException (409 Conflict).
            //        throw new HttpResponseException(HttpStatusCode.Conflict);
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}
            // (Replaced with an exception filter.

            return CreatedAtRoute("DefaultApi", new { id = reservation.ReservationId }, reservation);
        }

        // DELETE: api/Reservation/5
        [ResponseType(typeof(Reservation))]
        public IHttpActionResult DeleteReservation(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return NotFound();
            }

            db.Reservations.Remove(reservation);
            db.SaveChanges();

            return Ok(reservation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReservationExists(int id)
        {
            return db.Reservations.Count(e => e.ReservationId == id) > 0;
        }
    }
}
