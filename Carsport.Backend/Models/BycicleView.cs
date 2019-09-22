namespace Carsport.Backend.Models
{
    using System.Web;
    using Common.Models;

    public class BycicleView : Bycicle
    {
        public HttpPostedFileBase ImageFile { get; set; }
    }
}