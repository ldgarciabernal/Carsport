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
    public class MessagesController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Messages
        public IQueryable<Message> GetMessages()
        {
            return db.Messages;
        }

        // GET: api/Messages/5
        [ResponseType(typeof(Message))]
        public async Task<IHttpActionResult> GetMessage(int id)
        {
            Message message = await db.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            return Ok(message);
        }

        // PUT: api/Messages/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMessage(int id, Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != message.MessageId)
            {
                return BadRequest();
            }

            db.Entry(message).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Messages
        [ResponseType(typeof(Message))]
        public async Task<IHttpActionResult> PostMessage(Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var conversation = db.Conversations.FirstOrDefault(c => c.ConversationId == message.ConversationId);
            conversation.Messages.Add(message);
            db.Messages.Add(message);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = message.MessageId }, message);
        }

        // DELETE: api/Messages/5
        [ResponseType(typeof(Message))]
        public async Task<IHttpActionResult> DeleteMessage(int id)
        {
            Message message = await db.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            db.Messages.Remove(message);
            await db.SaveChangesAsync();

            return Ok(message);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MessageExists(int id)
        {
            return db.Messages.Count(e => e.MessageId == id) > 0;
        }
    }
}