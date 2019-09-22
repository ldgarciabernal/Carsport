using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Carsport.API.Helpers;
using Carsport.Common.Models;
using Carsport.Domain.Models;
using Newtonsoft.Json.Linq;

namespace Carsport.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/Routes")]
    public class RoutesController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Routes
        public IQueryable<Route> GetRoutes()
        {
            return db.Routes.Where(r => r.IsDeleted == false)
                .OrderByDescending(r => r.RouteId)
                .Include(r => r.Origin)
                .Include(r => r.Destiny)
                .Include(r => r.Conversations);
        }

        // GET: api/Routes/5
        [ResponseType(typeof(Route))]
        public async Task<IHttpActionResult> GetRoute(int id)
        {
            Route route = await db.Routes.FindAsync(id);
            if (route == null)
            {
                return NotFound();
            }

            return Ok(route);
        }

        // PUT: api/Routes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRoute(int id, Route route)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != route.RouteId)
            {
                return BadRequest();
            }

            if (route.ImageArray != null && route.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(route.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "~/Content/Routes";
                var fullPath = $"{folder}/{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    route.ImagePath = fullPath;
                }
            }

            var originEntity = db.Cities.Where(c => c.CityId == route.OriginID).ToList();
            var destinyEntity = db.Cities.Where(c => c.CityId == route.DestinyID).ToList();

            var oldRoute = await db.Routes.FindAsync(id);
            oldRoute.Conversations = route.Conversations;
            oldRoute.Date = route.Date;
            oldRoute.Description = route.Description;
            oldRoute.Destiny = destinyEntity[0];
            oldRoute.DestinyID = route.DestinyID;
            oldRoute.IsDeleted = route.IsDeleted;
            oldRoute.NumSeats = route.NumSeats;
            oldRoute.Origin = originEntity[0];
            oldRoute.OriginID = route.OriginID;
            oldRoute.ImagePath = route.ImagePath;

            db.ChangeTracker.DetectChanges();

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RouteExists(id))
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

        // POST: api/Routes
        [ResponseType(typeof(Route))]
        public async Task<IHttpActionResult> PostRoute(Route route)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (route.ImageArray != null && route.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(route.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "~/Content/Routes";
                var fullPath = $"{folder}/{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    route.ImagePath = fullPath;
                }
            }

            var originEntity = db.Cities.Where(c => c.CityId == route.OriginID).ToList();
            var destinyEntity = db.Cities.Where(c => c.CityId == route.DestinyID).ToList();
            route.Origin = originEntity[0];
            route.Destiny = destinyEntity[0];

            db.Routes.Add(route);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = route.RouteId }, route);
        }

        [HttpPost]
        [Authorize]
        [Route("GetRouteByEmail")]
        public IHttpActionResult GetRouteByEmail(JObject form)
        {
            var email = string.Empty;
            dynamic jsonObject = form;

            try
            {
                email = jsonObject.Email.Value;
            }
            catch
            {
                return BadRequest("Incorrect call.");
            }

            var userId = UsersHelper.GetUserASP(email);

            var routes = db.Routes.Where(r => r.IsDeleted == false && r.UserId == userId.Id).
                OrderByDescending(r => r.RouteId).Include(r => r.Origin).Include(r => r.Destiny).Include(c => c.Conversations);

            return Ok(routes);
        }

        // DELETE: api/Routes/5
        [ResponseType(typeof(Route))]
        public async Task<IHttpActionResult> DeleteRoute(int id)
        {
            Route route = await db.Routes.FindAsync(id);
            if (route == null)
            {
                return NotFound();
            }

            db.Routes.Remove(route);
            await db.SaveChangesAsync();

            return Ok(route);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RouteExists(int id)
        {
            return db.Routes.Count(e => e.RouteId == id) > 0;
        }
    }
}