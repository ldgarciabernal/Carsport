namespace Carsport.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Cores
    {
        [JsonProperty(PropertyName = "nucleos")]
        public List<Core> CoreList { get; set; }
    }

}
