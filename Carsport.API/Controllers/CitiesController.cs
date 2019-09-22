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
using Carsport.Common.Models;
using Carsport.Domain.Models;

namespace Carsport.API.Controllers
{
    [Authorize]
    public class CitiesController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Cities
        public IQueryable<City> GetCities()
        {
            return db.Cities.OrderBy(c => c.Name);
        }

        // GET: api/Cities/5
        [ResponseType(typeof(City))]
        public async Task<IHttpActionResult> GetCity(int id)
        {
            City city = await db.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            return Ok(city);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CityExists(int id)
        {
            return db.Cities.Count(e => e.CityId == id) > 0;
        }
    }
}