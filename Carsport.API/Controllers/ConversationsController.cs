using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Carsport.API.Helpers;
using Carsport.API.Models;
using Carsport.Common.Models;
using Carsport.Domain.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json.Linq;

namespace Carsport.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/Conversations")]
    public class ConversationsController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Conversations
        public IQueryable<Conversation> GetConversations()
        {
            return db.Conversations.Include(m => m.Messages);
        }

        // GET: api/Conversations/5
        [ResponseType(typeof(Conversation))]
        public async Task<IHttpActionResult> GetConversation(int id)
        {
            var conversation = db.Conversations.Where(c => c.ConversationId == id).Include(m => m.Messages);
            if (conversation == null)
            {
                return NotFound();
            }

            return Ok(conversation);
        }

        [HttpPost]
        [Route("GetMyConversations")]
        [Authorize]
        public IHttpActionResult GetMyConversations(JObject form)
        {
            try
            {
                var userId = string.Empty;
                dynamic jsonObject = form;

                try
                {
                    userId = jsonObject.UserId.Value;
                }
                catch (Exception ex)
                {
                    return BadRequest("Incorrect call.");
                }

                var converstion = db.Conversations
                    .Where(c => c.UserIdOne == userId || c.UserIdTwo == userId)
                    .Include(m => m.Messages);

                if (converstion == null)
                {
                    return NotFound();
                }

                var response = new List<ConversationUser>();
                foreach (var item in converstion)
                {
                    var me = UsersHelper.GetUserByIdASP(userId);
                    var users = (item.UserIdOne == me.Id) ? item.UserIdTwo : item.UserIdOne;
                    var userConver = UsersHelper.GetUserByIdASP(users);
                    var username = getNameClaim(userConver);
                    var surname = getSurnameClaim(userConver);


                    response.Add(new ConversationUser
                    {
                        ConversationId = item.ConversationId,
                        Email = userConver.Email,
                        UserName = (username != "" && surname != "") ? username + " " + surname : userConver.Email,
                        Messages = item.Messages,
                        UserIdOne = item.UserIdOne,
                        UserIdTwo = item.UserIdTwo,
                    });
                }

                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest("009. Server error");
            }
        }

        [HttpPost]
        [Route("GetRouteByUserRoute")]
        [Authorize]
        public IHttpActionResult GetRouteByUserRoute(JObject form)
        {
            try
            {
                var user = string.Empty;
                var routeId = 0;
                dynamic jsonObject = form;

                try
                {
                    user = jsonObject.User.Value;
                    routeId = int.Parse(jsonObject.RouteId.Value);
                }
                catch (Exception ex)
                {
                    return BadRequest("Incorrect call.");
                }

                var query = db.Routes
                             .Where(r => r.RouteId == routeId)
                             .SelectMany(x => x.Conversations)
                             .Where(c => c.UserIdOne == user || c.UserIdTwo == user)
                             .Include(m => m.Messages);

                if (query == null)
                {
                    return NotFound();
                }

                var response = new List<ConversationUser>();
                foreach (var item in query)
                {
                    var me = UsersHelper.GetUserByIdASP(user);
                    var users = (item.UserIdOne == me.Id) ? item.UserIdTwo : item.UserIdOne;
                    var userConver = UsersHelper.GetUserByIdASP(users);
                    var username = getNameClaim(userConver);
                    var surname = getSurnameClaim(userConver);

                    response.Add(new ConversationUser
                    {
                        ConversationId = item.ConversationId,
                        Email = userConver.Email,
                        UserName = (username != "" && surname != "") ? username + " " + surname : userConver.Email,
                        Messages = item.Messages,
                        UserIdOne = item.UserIdOne,
                        UserIdTwo = item.UserIdTwo,
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest("009. Server error");
            }
        }

        [HttpPost]
        [Route("GetByUsers")]
        public IHttpActionResult GetConversationByUsers(JObject form)
        {

            try
            {
                var userFrom = string.Empty;
                var userTo = string.Empty;
                dynamic jsonObject = form;

                try
                {
                    userFrom = jsonObject.UserFrom.Value;
                    userTo = jsonObject.UserTo.Value;
                }
                catch
                {
                    return BadRequest("Incorrect call.");
                }

                var converstion = db.Conversations
                    .Where(c => c.UserIdOne == userFrom && c.UserIdTwo == userTo || c.UserIdOne == userTo && c.UserIdTwo == userFrom)
                    .Include(m => m.Messages)
                    .Include(r => r.Routes);

                if (converstion == null)
                {
                    return NotFound();
                }

                return Ok(converstion);
            }
            catch (Exception)
            {
                return BadRequest("009. Server error");
            }
        }

        // PUT: api/Conversations/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutConversation(int id, Conversation conversation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != conversation.ConversationId)
            {
                return BadRequest();
            }

            db.Entry(conversation).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConversationExists(id))
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

        // POST: api/Conversations
        public async Task<IHttpActionResult> PostConversation(JObject form)
        {
            Conversation conversation = null;
            long routeId = 0;

            try
            {
                conversation = RequestToConversation(form);
                dynamic jsonObject = form;
                routeId = jsonObject.RouteId.Value;
            }
            catch (Exception ex)
            {
                return BadRequest(ModelState);
            }

            var route = db.Routes.Where(r => r.RouteId == routeId).Include(c => c.Conversations).ToList();
            route[0].Conversations.Add(conversation);
            db.Conversations.Add(conversation);
            db.SaveChanges();   

            return CreatedAtRoute("DefaultApi", new { id = conversation.ConversationId }, conversation);
        }

        private Conversation RequestToConversation(JObject form)
        {
            dynamic jsonObject = form;

            return new Conversation
            {
                UserIdOne = jsonObject.UserIdOne.Value,
                UserIdTwo = jsonObject.UserIdTwo.Value,
                Date = jsonObject.Date.Value,
                ConnectionId = jsonObject.ConnectionId.Value,
            };
        }

        // DELETE: api/Conversations/5
        [ResponseType(typeof(Conversation))]
        public async Task<IHttpActionResult> DeleteConversation(int id)
        {
            Conversation conversation = await db.Conversations.FindAsync(id);
            if (conversation == null)
            {
                return NotFound();
            }

            db.Conversations.Remove(conversation);
            await db.SaveChangesAsync();

            return Ok(conversation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ConversationExists(int id)
        {
            return db.Conversations.Count(e => e.ConversationId == id) > 0;
        }

        public string getNameClaim(ApplicationUser UserASP)
        {
            var name = "";

            foreach (IdentityUserClaim claim in UserASP.Claims.ToList())
            {
                if (claim.ClaimType == ClaimTypes.GivenName)
                {
                    name = claim.ClaimValue;
                }
            }

            return name;
        }

        public string getSurnameClaim(ApplicationUser UserASP)
        {
            var surName = "";

            foreach(IdentityUserClaim claim in UserASP.Claims.ToList())
            {
                if (claim.ClaimType == ClaimTypes.Name)
                {
                    surName = claim.ClaimValue;
                }
            }

            return surName;
        }
    }
}