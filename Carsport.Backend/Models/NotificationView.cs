 namespace Carsport.Backend.Models
{
    using Common.Models;
    using System.Web;

    public class NotificationView : Notification
    {
        public HttpPostedFileBase ImageFile { get; set; }
    }
}