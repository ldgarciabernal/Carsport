namespace Carsport.API.Helpers
{
    using Carsport.Domain.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using System.Web.Configuration;

    public class MailHelper
    {

        private static DataContext db = new DataContext();

        public static string getConfirmEmailSuccessfulMessage()
        {
            var confirmedMessage = "<html><head><meta name =\"viewport\" content=\"width=device-width\" />" +
                         "<title></title></head><body><div>";
            var mailType = WebConfigurationManager.AppSettings["ConfirmEmailSuccessfulType"].ToString().ToLower();
            var emailTemplate = db.MailTemplates.Where(m => m.Type.ToLower().Equals(mailType)).ToList();

            confirmedMessage += emailTemplate[0].Subject;
            confirmedMessage += emailTemplate[0].Body;

            confirmedMessage += "</div></body></html>";

            return confirmedMessage;
        }

        public static async Task SendMail(string to, string subject, string body)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(to));
            message.From = new MailAddress(WebConfigurationManager.AppSettings["AdminUser"]);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = WebConfigurationManager.AppSettings["AdminUser"],
                    Password = WebConfigurationManager.AppSettings["AdminPassWord"]
                };

                smtp.Credentials = credential;
                smtp.Host = WebConfigurationManager.AppSettings["SMTPName"];
                smtp.Port = int.Parse(WebConfigurationManager.AppSettings["SMTPPort"]);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
            }
        }

        public static async Task SendMail(List<string> mails, string subject, string body)
        {
            var message = new MailMessage();

            foreach (var to in mails)
            {
                message.To.Add(new MailAddress(to));
            }

            message.From = new MailAddress(WebConfigurationManager.AppSettings["AdminUser"]);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = WebConfigurationManager.AppSettings["AdminUser"],
                    Password = WebConfigurationManager.AppSettings["AdminPassWord"]
                };

                smtp.Credentials = credential;
                smtp.Host = WebConfigurationManager.AppSettings["SMTPName"];
                smtp.Port = int.Parse(WebConfigurationManager.AppSettings["SMTPPort"]);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
            }
        }
    }

}