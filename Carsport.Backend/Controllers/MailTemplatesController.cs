namespace Carsport.Backend.Controllers
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using System.Net;
    using System.Web.Mvc;
    using Carsport.Backend.Models;
    using Carsport.Common.Models;

    [Authorize(Roles = "Admin")]
    public class MailTemplatesController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: MailTemplates
        public async Task<ActionResult> Index()
        {
            return View(await db.MailTemplates.ToListAsync());
        }

        // GET: MailTemplates/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var mailTemplate = await db.MailTemplates.FindAsync(id);
            if (mailTemplate == null)
            {
                return HttpNotFound();
            }
            return View(mailTemplate);
        }

        // GET: MailTemplates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MailTemplates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create(MailTemplate mailTemplate)
        {
            if (ModelState.IsValid)
            {
                db.MailTemplates.Add(mailTemplate);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(mailTemplate);
        }

        // GET: MailTemplates/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var mailTemplate = await db.MailTemplates.FindAsync(id);
            if (mailTemplate == null)
            {
                return HttpNotFound();
            }
            return View(mailTemplate);
        }

        // POST: MailTemplates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(MailTemplate mailTemplate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mailTemplate).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(mailTemplate);
        }

        // GET: MailTemplates/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var mailTemplate = await db.MailTemplates.FindAsync(id);
            if (mailTemplate == null)
            {
                return HttpNotFound();
            }
            return View(mailTemplate);
        }

        // POST: MailTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var mailTemplate = await db.MailTemplates.FindAsync(id);
            db.MailTemplates.Remove(mailTemplate);
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
