namespace Carsport.Backend.Controllers
{
    using Backend.Models;
    using Carsport.Backend.Helpers;
    using Common.Models;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    [Authorize(Roles = "Admin")]
    public class ByciclesController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Bycicles
        public async Task<ActionResult> Index()
        {
            return View(await db.Bycicles.OrderByDescending(b => b.BycicleId).ToListAsync());
        }

        // GET: Bycicles/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bycicle bycicle = await db.Bycicles.FindAsync(id);
            if (bycicle == null)
            {
                return HttpNotFound();
            }
            return View(bycicle);
        }

        // GET: Bycicles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Bycicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BycicleView bycicleView)
        {
            if (ModelState.IsValid)
            {

                var pic = string.Empty;
                var folder = "~/Content/Bycicles";

                if (bycicleView.ImageFile != null)
                {
                    pic = FilesHelper.UploadPhoto(bycicleView.ImageFile, folder);
                    pic = $"{folder}/{pic}";
                }

                var bycicle = this.ToBycicle(bycicleView, pic);

                db.Bycicles.Add(bycicle);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(bycicleView);
        }

        private Bycicle ToBycicle(BycicleView bycicleView, string image)
        {
            return new Bycicle
            {
                BycicleId = bycicleView.BycicleId,
                Description = bycicleView.Description,
                ImagePath = image,
                IsAvailable = bycicleView.IsAvailable,
                Latitude = bycicleView.Latitude,
                Longitude = bycicleView.Longitude,
                Street = bycicleView.Street,
                University = bycicleView.University,
            };
        }

        // GET: Bycicles/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var bycicle = await db.Bycicles.FindAsync(id);

            if (bycicle == null)
            {
                return HttpNotFound();
            }

            var view = this.ToView(bycicle);
            return View(view);
        }

        private BycicleView ToView(Bycicle bycicle)
        {
            return new BycicleView
            {
                BycicleId = bycicle.BycicleId,
                Description = bycicle.Description,
                ImagePath = bycicle.ImagePath,
                IsAvailable = bycicle.IsAvailable,
                Latitude = bycicle.Latitude,
                Longitude = bycicle.Longitude,
                Street = bycicle.Street,
                University = bycicle.University,
            };
        }

        // POST: Bycicles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BycicleView bycicleView)
        {
            if (ModelState.IsValid)
            {

                var pic = bycicleView.ImagePath;
                var folder = "~/Content/Bycicles";

                if (bycicleView.ImageFile != null)
                {
                    pic = FilesHelper.UploadPhoto(bycicleView.ImageFile, folder);
                    pic = $"{folder}/{pic}";
                }

                var bycicle = this.ToBycicle(bycicleView, pic);
                db.Entry(bycicle).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(bycicleView);
        }

        // GET: Bycicles/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bycicle bycicle = await db.Bycicles.FindAsync(id);
            if (bycicle == null)
            {
                return HttpNotFound();
            }
            return View(bycicle);
        }

        // POST: Bycicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Bycicle bycicle = await db.Bycicles.FindAsync(id);
            db.Bycicles.Remove(bycicle);
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
