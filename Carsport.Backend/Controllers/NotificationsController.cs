namespace Carsport.Backend.Controllers
{
    using Carsport.Backend.Helpers;
    using Carsport.Backend.Models;
    using Carsport.Common.Models;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    [Authorize(Roles = "Admin")]
    public class NotificationsController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Notifications
        public async Task<ActionResult> Index()
        {
            return View(await db.Notifications.OrderByDescending(n => n.PublishedOn).ToListAsync());
        }

        // GET: Notifications/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = await db.Notifications.FindAsync(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            
            return View(notification);
        }

        // GET: Notifications/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Notifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(NotificationView notificationView)
        {
            if (ModelState.IsValid)
            {
                notificationView.PublishedOn = DateTime.Now.ToLocalTime();
                notificationView.UserId = User.Identity.GetUserId();

                var pic = string.Empty;
                var folder = "~/Content/Notifications";

                if (notificationView.ImageFile != null)
                {
                    pic = FilesHelper.UploadPhoto(notificationView.ImageFile, folder);
                    pic = $"{folder}/{pic}";
                }

                var notification = this.ToNotification(notificationView, pic);

                db.Notifications.Add(notification);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(notificationView);
        }

        private Notification ToNotification(NotificationView notificationView, string pic)
        {
            return new Notification
            {
                NotificationId = notificationView.NotificationId,
                Title = notificationView.Title,
                Body = notificationView.Body,
                IsAvailable = notificationView.IsAvailable,
                PublishedOn = notificationView.PublishedOn,
                UserId = notificationView.UserId,
                ImagePath = pic,
            };
        }

        // GET: Notifications/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = await db.Notifications.FindAsync(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            var view = this.ToView(notification);
            return View(view);
        }

        private NotificationView ToView(Notification notification)
        {
            return new NotificationView
            {
                NotificationId = notification.NotificationId,
                Title = notification.Title,
                Body = notification.Body,
                IsAvailable = notification.IsAvailable,
                PublishedOn = notification.PublishedOn,
                ImagePath = notification.ImagePath,
            };
        }

        // POST: Notifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(NotificationView notificationView)
        {
            if (ModelState.IsValid)
            {
                notificationView.PublishedOn = DateTime.Now.ToLocalTime();
                var pic = notificationView.ImagePath;
                var folder = "~/Content/Notifications";

                if (notificationView.ImageFile != null)
                {
                    pic = FilesHelper.UploadPhoto(notificationView.ImageFile, folder);
                    pic = $"{folder}/{pic}";
                }

                var notification = this.ToNotification(notificationView, pic);

                db.Entry(notification).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(notificationView);
        }

        // GET: Notifications/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = await db.Notifications.FindAsync(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            return View(notification);
        }

        // POST: Notifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Notification notification = await db.Notifications.FindAsync(id);
            db.Notifications.Remove(notification);
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
