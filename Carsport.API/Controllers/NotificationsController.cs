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
    public class NotificationsController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Notifications
        public IQueryable<Notification> GetNotifications()
        {
            return db.Notifications.Where(n => n.IsAvailable == true).OrderBy(n => n.PublishedOn);
        }

        // GET: api/Notifications/5
        [ResponseType(typeof(Notification))]
        public async Task<IHttpActionResult> GetNotification(int id)
        {
            Notification notification = await db.Notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }

            return Ok(notification);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NotificationExists(int id)
        {
            return db.Notifications.Count(e => e.NotificationId == id) > 0;
        }
    }
}