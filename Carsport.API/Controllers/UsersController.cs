namespace Carsport.API.Controllers
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using Common.Models;
    using Helpers;
    using Newtonsoft.Json.Linq;

    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        public async Task<IHttpActionResult> PostUser(UserRequest userRequest)
        {
            if (userRequest.ImageArray != null && userRequest.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(userRequest.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "~/Content/Users";
                var fullPath = $"{folder}/{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    userRequest.ImagePath = fullPath;
                }
            }

            var answer = UsersHelper.CreateUserASP(userRequest);
            if(answer.IsSuccess)
            {
                await UsersHelper.ConfirmationEmail(userRequest.EMail, Url);
                answer.Message = errorMessage;

                return Ok(answer);
            }
            return BadRequest(answer.Message);
        }

        public static string errorMessage;

        [HttpPost]
        [Authorize]
        [Route("GetUser")]
        public IHttpActionResult GetUser(JObject form)
        {
            try
            {
                var email = string.Empty;
                dynamic jsonObject = form;

                try
                {
                    email = jsonObject.Email.Value;
                }
                catch
                {
                    return BadRequest("Incorrect call.");
                }

                var user = UsersHelper.GetUserASP(email);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("GetUserById")]
        public IHttpActionResult GetUserById(JObject form)
        {
            try
            {
                var userId = string.Empty;
                dynamic jsonObject = form;

                try
                {
                    userId = jsonObject.Email.Value;
                }
                catch
                {
                    return BadRequest("Incorrect call.");
                }

                var user = UsersHelper.GetUserByIdASP(userId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("Delete")]
        public IHttpActionResult DeleteUser(JObject form)
        {
            try
            {
                var email = string.Empty;
                dynamic jsonObject = form;

                try
                {
                    email = jsonObject.Email.Value;
                }
                catch
                {
                    return BadRequest("Incorrect call.");
                }

                var user = UsersHelper.GetUserASP(email);
                if (user == null)
                    return BadRequest("006. User not found");

                var userRole = UsersHelper.GetUserRole(user.Email);

                if (userRole != null)
                {
                    var isDeleted = UsersHelper.DeleteUser(user.Email, userRole);
                    var isDeletedClaim = UsersHelper.DeleteUserClaims(user.Email);
                    var isDeletedAccaunt = UsersHelper.DeleteUserAccount(user.Email);
                }
                else
                {
                    return BadRequest("002. User cant be delete");
                }
                
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize]
        [Route("Put")]
        public IHttpActionResult PutUser(UserRequest userRequest)
        {
            try
            {
                var susscefull = UsersHelper.UpdateUserClaims(userRequest);

                return Ok(susscefull);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("RecoveryPassword")]
        public async Task<IHttpActionResult> PostRecoveryPassword(JObject form)
        {
            try
            {
                var email = string.Empty;
                dynamic jsonObject = form;

                try
                {
                    email = jsonObject.Email.Value;
                }
                catch
                {
                    return BadRequest("Incorrect call.");
                }

                var user = await UsersHelper.PasswordRecovery(email);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("ConfirmEmail")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<HttpResponseMessage> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            
            code = HttpUtility.UrlDecode(code);
            var result = await UsersHelper.ConfirmEmail(userId, code);

            if (!result)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var response = new HttpResponseMessage();
            response.Content = new StringContent(MailHelper.getConfirmEmailSuccessfulMessage());
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }

        [HttpPost]
        [Authorize]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> PostChangePassword(JObject form)
        {
            try
            {
                var email = string.Empty;
                var password = string.Empty;
                dynamic jsonObject = form;

                try
                {
                    email = jsonObject.Email.Value;
                    password = jsonObject.Password.Value;
                }
                catch
                {
                    return BadRequest("Incorrect call.");
                }

                var user = UsersHelper.GetUserASP(email);
                if (user == null)
                {
                    return BadRequest("003. User dont exist.");
                }

                var result = await UsersHelper.ChangePassword(user.Id, password);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
