using Carsport.Backend.Helpers;
using Carsport.Backend.Models;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Carsport.Backend.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            var users = UsersHelper.GetUserList();
            var userView = users.Select(r => new UserView
            {
                UserId = r.Id,
                EMail = r.Email,
                FirstName = r.Claims.FirstOrDefault(c => c.ClaimType == ClaimTypes.GivenName).ClaimValue,
                LastName = r.Claims.FirstOrDefault(c => c.ClaimType == ClaimTypes.Name).ClaimValue,
                ImagePath = r.Claims.FirstOrDefault(c => c.ClaimType == ClaimTypes.Uri) != null ? $"http://movilidaducaapi.somee.com{r.Claims.FirstOrDefault(c => c.ClaimType == ClaimTypes.Uri).ClaimValue.Substring(1)}" : "http://movilidaducabackend.somee.com/Content/Media/no_profile.png",
                EmailConfirmed = r.EmailConfirmed,
            }).OrderByDescending(u => u.UserId).ToList();
            return View(userView);
        }

        // GET: Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = UsersHelper.GetUserByIdASP(id);
            
            if (user == null)
            {
                return HttpNotFound();
            }

            var userView = this.ToUserView(user);
            return View(userView);
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = UsersHelper.GetUserByIdASP(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var userView = this.ToUserView(user);
            return View(userView);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var user = UsersHelper.GetUserByIdASP(id);
            var isDeletd = UsersHelper.DeleteUser(user.Email, "User");
            var isDeletdClaims = UsersHelper.DeleteUserClaims(user.Email);
            var isDeletdAccount = UsersHelper.DeleteUserAccount(user.Email);
            if(isDeletd && isDeletdAccount && isDeletdAccount)
            {
                return RedirectToAction("Index");
            }
            
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        private UserView ToUserView(ApplicationUser user)
        {
            return new UserView
            {
                UserId = user.Id,
                EMail = user.Email,
                FirstName = user.Claims.FirstOrDefault(c => c.ClaimType == ClaimTypes.GivenName).ClaimValue,
                LastName = user.Claims.FirstOrDefault(c => c.ClaimType == ClaimTypes.Name).ClaimValue,
                ImagePath = user.Claims.FirstOrDefault(c => c.ClaimType == ClaimTypes.Uri) != null ? $"{WebConfigurationManager.AppSettings["apiUrl"]}{user.Claims.FirstOrDefault(c => c.ClaimType == ClaimTypes.Uri).ClaimValue.Substring(1)}" : "http://movilidaducabackend.somee.com/Content/Media/no_profile.png",
                EmailConfirmed = user.EmailConfirmed,
            };
        }
    }
}