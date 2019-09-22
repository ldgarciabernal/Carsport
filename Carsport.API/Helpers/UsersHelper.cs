namespace Carsport.API.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Configuration;
    using System.Web.Http.Routing;
    using System.Web.Security;
    using Carsport.API.Controllers;
    using Common.Models;
    using Domain.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security.DataProtection;

    public class UsersHelper : IDisposable
    {
        private static ApplicationDbContext userContext = new ApplicationDbContext();
        private static DataContext db = new DataContext();

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

        public static string GetUserRole(string email)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(email);
            var role = userManager.GetRoles(userASP.Id).FirstOrDefault();

            return role;
        }

        public static bool UpdateUserClaims(UserRequest user)
        {
            var isRemove = true;
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(user.EMail);
            if (userASP == null)
            {
                return false;
            }

            var claims = userManager.GetClaims(userASP.Id);
            
            if (!string.IsNullOrEmpty(user.FirstName))
            {
                var nameClaim = GetClaimByType(claims, ClaimTypes.GivenName);

                if (nameClaim != null)
                    userManager.RemoveClaim(userASP.Id, (System.Security.Claims.Claim)nameClaim);
                userManager.AddClaim(userASP.Id, new System.Security.Claims.Claim(ClaimTypes.GivenName, user.FirstName));
            }

            if (!string.IsNullOrEmpty(user.LastName))
            {
                var lastnameClaim = GetClaimByType(claims, ClaimTypes.Name);

                if (lastnameClaim != null)
                    userManager.RemoveClaim(userASP.Id, (System.Security.Claims.Claim)lastnameClaim);
                userManager.AddClaim(userASP.Id, new System.Security.Claims.Claim(ClaimTypes.Name, user.LastName));
            }

            if (user.ImageArray != null && user.ImageArray.Length > 0)
            {
                var imageClaim = GetClaimByType(claims, ClaimTypes.Uri);

                if (imageClaim != null)
                    userManager.RemoveClaim(userASP.Id, (System.Security.Claims.Claim)imageClaim);

                var stream = new MemoryStream(user.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "~/Content/Users";
                var fullPath = $"{folder}/{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    user.ImagePath = fullPath;
                }

                userManager.AddClaim(userASP.Id, new System.Security.Claims.Claim(ClaimTypes.Uri, user.ImagePath));
            }

            userASP.EmailConfirmed = true;

            return isRemove;
        }

        private static object GetClaimByType(IList<System.Security.Claims.Claim> claims, string type)
        {
            foreach (var item in claims)
            {
                if (item.Type.Equals(type))
                {
                    return item;
                }
            }

            return null;
        }

        public static ApplicationUser GetUserASP(string email)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(email);
            return userASP;
        }

        public static ApplicationUser GetUserByIdASP(string id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindById(id);
            return userASP;
        }

        public static Response CreateUserASP(UserRequest userRequest)
        {
            try
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
                var oldUserASP = userManager.FindByEmail(userRequest.EMail);
                if (oldUserASP != null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "001. User already exists.",
                    };
                }

                var userASP = new ApplicationUser
                {
                    Email = userRequest.EMail,
                    UserName = userRequest.EMail,
                };

                var result = userManager.Create(userASP, userRequest.Password);
                if (result.Succeeded)
                {
                    var newUserASP = userManager.FindByEmail(userRequest.EMail);
                    userManager.AddClaim(newUserASP.Id, new System.Security.Claims.Claim(ClaimTypes.GivenName, userRequest.FirstName));
                    userManager.AddClaim(newUserASP.Id, new System.Security.Claims.Claim(ClaimTypes.Name, userRequest.LastName));

                    if (!string.IsNullOrEmpty(userRequest.ImagePath))
                    {
                        userManager.AddClaim(newUserASP.Id, new System.Security.Claims.Claim(ClaimTypes.Uri, userRequest.ImagePath));
                    }
                    
                    userManager.AddToRole(newUserASP.Id, "User");

                    return new Response
                    {
                        IsSuccess = true,
                    };
                }

                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error}, ";
                }

                return new Response
                {
                    IsSuccess = false,
                    Message = errors,
                };
            }
            catch (Exception ex)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
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

        public static async Task<bool> PasswordRecovery(string email)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(email);
            if (userASP == null)
            {
                return false;
            }

            var random = new Random();
            var newPassword = Membership.GeneratePassword(8, 3);
            var machineKeyProtectionProvider = new MachineKeyProtectionProvider();
            userManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(machineKeyProtectionProvider.Create("PasswordRecover"));

            var token = await userManager.GeneratePasswordResetTokenAsync(userASP.Id);
            var response = await userManager.ResetPasswordAsync(userASP.Id, token, newPassword);
            if (response.Succeeded)
            {

                var mailType = WebConfigurationManager.AppSettings["ResetPasswordEmailType"].ToString().ToLower();
                var emailTemplate = db.MailTemplates.Where(m => m.Type.ToLower().Equals(mailType)).ToList();

                var subject = emailTemplate[0].Subject;
                var body = emailTemplate[0].Body.Replace("{##}", newPassword);

                await MailHelper.SendMail(email, subject, body);
            }

            return true;
        }

        public static async Task ConfirmationEmail(string email, UrlHelper url)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(email);
            if (userASP == null)
            {
                return;
            }
            
            try
            {
                var machineKeyProtectionProvider = new MachineKeyProtectionProvider();
                userManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(machineKeyProtectionProvider.Create("EmailConfirmation"));
                
                string code = await userManager.GenerateEmailConfirmationTokenAsync(userASP.Id);
                code = HttpUtility.UrlEncode(code);

                string callbackUrl = url.Link("DefaultApi", new { controller = "Users/ConfirmEmail", userId = userASP.Id, code });

                var mailType = WebConfigurationManager.AppSettings["ConfirmEmailType"].ToString().ToLower();
                var emailTemplate = db.MailTemplates.Where(m => m.Type.ToLower().Equals(mailType)).ToList();

                var subject = emailTemplate[0].Subject;
                var body = emailTemplate[0].Body.Replace("{##}", callbackUrl);

                await MailHelper.SendMail(email, subject, body);
            }
            catch (Exception e) {
                UsersController.errorMessage = e.Message;
                return;
            }
        }

        public static async Task<bool> ConfirmEmail(string userId, string code)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var machineKeyProtectionProvider = new MachineKeyProtectionProvider();
            userManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(machineKeyProtectionProvider.Create("EmailConfirmation"));

            var result = await userManager.ConfirmEmailAsync(userId, code);
            return result.Succeeded;
        }

        public static async Task<bool> ChangePassword(string userId, string password)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            var machineKeyProtectionProvider = new MachineKeyProtectionProvider();
            userManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(machineKeyProtectionProvider.Create("ResetPassword"));

            var token = await userManager.GeneratePasswordResetTokenAsync(userId);
            var response = await userManager.ResetPasswordAsync(userId, token, password);
            return response.Succeeded;
        }

        public void Dispose()
        {
            userContext.Dispose();
            db.Dispose();
        }
    }

    internal class MachineKeyProtectionProvider : IDataProtectionProvider
    {
        public IDataProtector Create(params string[] purposes)
        {
            return new MachineKeyDataProtector(purposes);
        }
    }

    internal class MachineKeyDataProtector : IDataProtector
    {
        private readonly string[] _purposes;

        public MachineKeyDataProtector(string[] purposes)
        {
            _purposes = purposes;
        }

        public byte[] Protect(byte[] userData)
        {
            return MachineKey.Protect(userData, _purposes);
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            return MachineKey.Unprotect(protectedData, _purposes);
        }
    }

}