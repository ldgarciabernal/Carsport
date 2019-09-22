namespace Carsport.API.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Common.Models;
    using Domain.Models;

    [Authorize]
    public class ByciclesController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Bycicles
        public IQueryable<Bycicle> GetBycicles()
        {
            return db.Bycicles;
        }

        // GET: api/Bycicles/5
        [ResponseType(typeof(Bycicle))]
        public async Task<IHttpActionResult> GetBycicle(int id)
        {
            Bycicle bycicle = await db.Bycicles.FindAsync(id);
            if (bycicle == null)
            {
                return NotFound();
            }

            return Ok(bycicle);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BycicleExists(int id)
        {
            return db.Bycicles.Count(e => e.BycicleId == id) > 0;
        }
    }
}