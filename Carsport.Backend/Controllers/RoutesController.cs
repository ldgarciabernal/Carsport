namespace Carsport.Backend.Controllers
{
    using Carsport.Backend.Helpers;
    using Carsport.Backend.Models;
    using Carsport.Common.Models;
    using Microsoft.AspNet.Identity;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    [Authorize(Roles = "Admin")]
    public class RoutesController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Routes
        public async Task<ActionResult> Index()
        {
            var routes = db.Routes.Include(r => r.Origin).Include(r => r.Destiny);
            return View(await routes.ToListAsync());
        }

        // GET: Routes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Route route = await db.Routes.FindAsync(id);
            if (route == null)
            {
                return HttpNotFound();
            }
            return View(route);
        }

        // GET: Routes/Create
        public ActionResult Create()
        {
            ViewBag.OriginID = new SelectList(db.Cities, "CityId", "Name");
            ViewBag.DestinyID = new SelectList(db.Cities, "CityId", "Name");
            return View();
        }

        // POST: Routes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RouteView routeView)
        {
            if (ModelState.IsValid)
            {
                var originEntity = db.Cities.Where(c => c.CityId == routeView.OriginID).ToList();
                var destinyEntity = db.Cities.Where(c => c.CityId == routeView.DestinyID).ToList();

                routeView.Origin = originEntity[0];
                routeView.Destiny = destinyEntity[0];
                routeView.UserId = User.Identity.GetUserId();

                var pic = string.Empty;
                var folder = "~/Content/Routes";

                if (routeView.ImageFile != null)
                {
                    pic = FilesHelper.UploadPhoto(routeView.ImageFile, folder);
                    pic = $"{folder}/{pic}";
                }

                var route = this.ToRoute(routeView, pic);

                db.Routes.Add(route);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.OriginID = new SelectList(db.Cities, "CityId", "Name");
            ViewBag.DestinyID = new SelectList(db.Cities, "CityId", "Name");
            return View(routeView);
        }

        // GET: Routes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Route route = await db.Routes.FindAsync(id);
            if (route == null)
            {
                return HttpNotFound();
            }

            ViewBag.OriginID = new SelectList(db.Cities, "CityId", "Name", route.OriginID);
            ViewBag.DestinyID = new SelectList(db.Cities, "CityId", "Name", route.DestinyID);

            var view = this.ToView(route);

            return View(view);
        }

        // POST: Routes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RouteView routeView)
        {
            if (ModelState.IsValid)
            {
                var originEntity = db.Cities.Where(c => c.CityId == routeView.OriginID).ToList();
                var destinyEntity = db.Cities.Where(c => c.CityId == routeView.DestinyID).ToList();

                routeView.Origin = originEntity[0];
                routeView.Destiny = destinyEntity[0];

                var pic = routeView.ImagePath;
                var folder = "~/Content/Routes";

                if (routeView.ImageFile != null)
                {
                    pic = FilesHelper.UploadPhoto(routeView.ImageFile, folder);
                    pic = $"{folder}/{pic}";
                }

                var route = this.ToRoute(routeView, pic);

                var oldRoute = await db.Routes.FindAsync(route.RouteId);
                oldRoute.Conversations = route.Conversations;
                oldRoute.Date = route.Date;
                oldRoute.Description = oldRoute.Description;
                oldRoute.Destiny = destinyEntity[0];
                oldRoute.DestinyID = route.DestinyID;
                oldRoute.IsDeleted = route.IsDeleted;
                oldRoute.NumSeats = route.NumSeats;
                oldRoute.Origin = originEntity[0];
                oldRoute.OriginID = route.OriginID;
                oldRoute.ImagePath = route.ImagePath;

                db.ChangeTracker.DetectChanges();
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.OriginID = new SelectList(db.Cities, "CityId", "Name", routeView.OriginID);
            ViewBag.DestinyID = new SelectList(db.Cities, "CityId", "Name", routeView.DestinyID);
            return View(routeView);
        }

        // GET: Routes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Route route = await db.Routes.FindAsync(id);
            if (route == null)
            {
                return HttpNotFound();
            }
            return View(route);
        }

        // POST: Routes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Route route = await db.Routes.FindAsync(id);
            db.Routes.Remove(route);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private Route ToRoute(RouteView routeView, string pic)
        {
            return new Route
            {
                RouteId = routeView.RouteId,
                OriginID = routeView.OriginID,
                DestinyID = routeView.DestinyID,
                Description = routeView.Description,
                Date = routeView.Date,
                IsDeleted = routeView.IsDeleted,
                UserId = routeView.UserId,
                Origin = routeView.Origin,
                Destiny = routeView.Destiny,
                Conversations = routeView.Conversations,
                NumSeats = routeView.NumSeats,
                Inbackend = true,
                ImagePath = pic,
            };
        }

        private RouteView ToView(Route route)
        {
            return new RouteView
            {
                RouteId = route.RouteId,
                OriginID = route.OriginID,
                DestinyID = route.DestinyID,
                Description = route.Description,
                Date = route.Date,
                IsDeleted = route.IsDeleted,
                UserId = route.UserId,
                Origin = route.Origin,
                Destiny = route.Destiny,
                Inbackend = route.Inbackend,
                Conversations = route.Conversations,
                NumSeats = route.NumSeats,
                ImagePath = route.ImagePath,
            };
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
