namespace Carsport.Backend.Helpers
{
    using Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Threading.Tasks;
    using System.Web.Configuration;
    using System.Linq;
    using System.Collections.Generic;
    using System.Data.Entity;

    public class UsersHelper : IDisposable
    {
        private static ApplicationDbContext userContext = new ApplicationDbContext();
        private static LocalDataContext db = new LocalDataContext();

        public static bool DeleteUser(string userName, string roleName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(userName);
            if (userASP == null)
            {
                return false;
            }

            var response = userManager.RemoveFromRole(userASP.Id, roleName);
            return response.Succeeded;
        }

        public static bool DeleteUserClaims(string userName)
        {
            var isRemove = true;
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(userName);
            if (userASP == null)
            {
                return false;
            }

            var claims = userManager.GetClaims(userASP.Id);
            foreach (var claim in claims)
            {
                var response = userManager.RemoveClaim(userASP.Id, claim);

                if (!response.Succeeded)
                    isRemove = false;
            }

            return isRemove;
        }

        public static bool DeleteUserAccount(string userName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(userName);
            if (userASP == null)
            {
                return false;
            }

            var response = userManager.Delete(userASP);

            return response.Succeeded;
        }

        public static bool UpdateUserName(string currentUserName, string newUserName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(currentUserName);
            if (userASP == null)
            {
                return false;
            }

            userASP.UserName = newUserName;
            userASP.Email = newUserName;
            var response = userManager.Update(userASP);
            return response.Succeeded;
        }

        public static void CheckRole(string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(userContext));

            // Check to see if Role Exists, if not create it
            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new IdentityRole(roleName));
            }
        }

        public static void CheckSuperUser()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var email = WebConfigurationManager.AppSettings["AdminUser"];
            var password = WebConfigurationManager.AppSettings["AdminPassWord"];
            var userASP = userManager.FindByName(email);
            if (userASP == null)
            {
                CreateUserASP(email, "Admin", password);
                return;
            }
        }

        public static void CreateUserASP(string email, string roleName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(email);
            if (userASP == null)
            {
                userASP = new ApplicationUser
                {
                    Email = email,
                    UserName = email,
                };

                userManager.Create(userASP, email);
            }

            userManager.AddToRole(userASP.Id, roleName);
        }

        public static void CreateUserASP(string email, string roleName, string password)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            var userASP = new ApplicationUser
            {
                Email = email,
                UserName = email,
            };

            var result = userManager.Create(userASP, password);
            if (result.Succeeded)
            {
                userManager.AddToRole(userASP.Id, roleName);
            }
        }

        public static async Task PasswordRecovery(string email)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(email);
            if (userASP == null)
            {
                return;
            }

            var random = new Random();
            var newPassword = string.Format("{0}", random.Next(100000, 999999));
            var response = await userManager.AddPasswordAsync(userASP.Id, newPassword);
            if (response.Succeeded)
            {
                var subject = "Lands App - Recuperación de contraseña";
                var body = string.Format(@"
                    <h1>Lands App - Recuperación de contraseña</h1>
                    <p>Su nueva contraseña es: <strong>{0}</strong></p>
                    <p>Por favor no olvide cambiarla por una de fácil recordación",
                    newPassword);

                await MailHelper.SendMail(email, subject, body);
            }
        }

        public static List<ApplicationUser> GetUserList()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var roleId = userContext.Roles.Where(m => m.Name == "User").Select(m => m.Id).SingleOrDefault();
            return userManager.Users.Include(r => r.Roles).Where(u => u.Roles.Any(r => r.RoleId == roleId)).ToList();
        }

        public static ApplicationUser GetUserByIdASP(string id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindById(id);
            return userASP;
        }

        public void Dispose()
        {
            userContext.Dispose();
            db.Dispose();
        }
    }
}