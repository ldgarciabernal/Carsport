using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Carsport.Common.Models;
using Carsport.Backend.Models;

namespace Carsport.Backend.Controllers
{
    public class ConversationsController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Conversations
        public async Task<ActionResult> Index()
        {
            var conversation = db.Conversations.Include(m => m.Messages).Include(r => r.Routes);
            return View(await conversation.ToListAsync());
        }

        // GET: Conversations/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conversation conversation = await db.Conversations.FindAsync(id);
            if (conversation == null)
            {
                return HttpNotFound();
            }
            return View(conversation);
        }

        // GET: Conversations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Conversations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Conversation conversation)
        {
            if (ModelState.IsValid)
            {
                db.Conversations.Add(conversation);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(conversation);
        }

        // GET: Conversations/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conversation conversation = await db.Conversations.FindAsync(id);
            if (conversation == null)
            {
                return HttpNotFound();
            }
            return View(conversation);
        }

        // POST: Conversations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Conversation conversation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(conversation).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(conversation);
        }

        // GET: Conversations/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conversation conversation = await db.Conversations.FindAsync(id);
            if (conversation == null)
            {
                return HttpNotFound();
            }
            return View(conversation);
        }

        // POST: Conversations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Conversation conversation = await db.Conversations.FindAsync(id);
            db.Conversations.Remove(conversation);
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
