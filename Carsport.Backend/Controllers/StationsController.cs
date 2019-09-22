namespace Carsport.Backend.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Backend.Models;
    using Common.Models;

    [Authorize(Roles = "Admin")]
    public class StationsController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Stations
        public async Task<ActionResult> Index()
        {
            return View(await db.Stations.OrderBy(s => s.Name).ToListAsync());
        }

        // GET: Stations/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Station station = await db.Stations.FindAsync(id);
            if (station == null)
            {
                return HttpNotFound();
            }
            return View(station);
        }

        // GET: Stations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Station station)
        {
            if (ModelState.IsValid)
            {
                db.Stations.Add(station);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(station);
        }

        // GET: Stations/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var station = await db.Stations.FindAsync(id);
            if (station == null)
            {
                return HttpNotFound();
            }
            return View(station);
        }

        // POST: Stations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Station station)
        {
            if (ModelState.IsValid)
            {
                db.Entry(station).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(station);
        }

        // GET: Stations/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var station = await db.Stations.FindAsync(id);
            if (station == null)
            {
                return HttpNotFound();
            }
            return View(station);
        }

        // POST: Stations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var station = await db.Stations.FindAsync(id);
            db.Stations.Remove(station);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
