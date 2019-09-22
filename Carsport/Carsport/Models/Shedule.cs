namespace Carsport.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Shedule
    {
        [JsonProperty(PropertyName = "idlinea")]
        public string LineId { get; set; }

        [JsonProperty(PropertyName = "codigo")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "horas")]
        public List<string> Hours { get; set; }

        [JsonProperty(PropertyName = "dias")]
        public string Days { get; set; }

        [JsonProperty(PropertyName = "observaciones")]
        public string Remarks { get; set; }

        [JsonProperty(PropertyName = "demandahoras")]
        public string HoursDemand { get; set; }
    }

}
