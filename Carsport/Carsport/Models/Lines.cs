namespace Carsport.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class Lines
    {
        [JsonProperty(PropertyName = "lineas")]
        public List<Line> LineList { get; set; }
    }
}
