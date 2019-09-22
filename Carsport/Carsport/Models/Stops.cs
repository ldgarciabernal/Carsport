namespace Carsport.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class Stops
    {
        [JsonProperty(PropertyName = "paradas")]
        public List<Stop> StopList { get; set; }
    }
}
