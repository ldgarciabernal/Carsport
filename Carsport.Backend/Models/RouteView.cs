using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carsport.Backend.Models
{
    using Common.Models;
    using System.Web;

    public class RouteView : Route
    {
        public HttpPostedFileBase ImageFile { get; set; }
    }
}