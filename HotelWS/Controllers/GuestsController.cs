using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using HotelWS;

namespace HotelWS.Controllers
{
    public class GuestsController : ApiController
    {
        private HotelDBContext db = new HotelDBContext();

        // GET: api/Guests
        public IQueryable<Guest> GetGuests()
        {
            return db.Guests;
        }

        // GET: api/Guests/5
        [ResponseType(typeof(Guest))]
        public async Task<IHttpActionResult> GetGuest(int id)
        {
            Guest guest = await db.Guests.FindAsync(id);
            if (guest == null)
            {
                return NotFound();
            }

            return Ok(guest);
        }

        // PUT: api/Guests/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutGuest(int id, Guest guest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != guest.Guest_No)
            {
                return BadRequest();
            }

            db.Entry(guest).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GuestExists(id))
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

        // POST: api/Guests
        [ResponseType(typeof(Guest))]
        public async Task<IHttpActionResult> PostGuest(Guest guest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Guests.Add(guest);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (GuestExists(guest.Guest_No))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = guest.Guest_No }, guest);
        }

        // DELETE: api/Guests/5
        [ResponseType(typeof(Guest))]
        public async Task<IHttpActionResult> DeleteGuest(int id)
        {
            Guest guest = await db.Guests.FindAsync(id);
            if (guest == null)
            {
                return NotFound();
            }

            db.Guests.Remove(guest);
            await db.SaveChangesAsync();

            return Ok(guest);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GuestExists(int id)
        {
            return db.Guests.Count(e => e.Guest_No == id) > 0;
        }
    }
}