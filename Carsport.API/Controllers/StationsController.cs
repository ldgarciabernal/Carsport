namespace Carsport.API.Controllers
{
    using Carsport.Common.Models;
    using Carsport.Domain.Models;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    public class StationsController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Stations
        public IQueryable<Station> GetStations()
        {
            return db.Stations.OrderBy(s => s.Name);
        }

        // GET: api/Stations/5
        [ResponseType(typeof(Station))]
        public async Task<IHttpActionResult> GetStation(int id)
        {
            Station station = await db.Stations.FindAsync(id);
            if (station == null)
            {
                return NotFound();
            }

            return Ok(station);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StationExists(int id)
        {
            return db.Stations.Count(e => e.Id == id) > 0;
        }
    }
}