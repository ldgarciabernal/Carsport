namespace Carsport.Models
{
    using Newtonsoft.Json;

    public class Frequency
    {
        [JsonProperty(PropertyName = "idfrecuencia")]
        public string FrequencyId { get; set; }

        [JsonProperty(PropertyName = "acronimo")]
        public string Acronym { get; set; }

        [JsonProperty(PropertyName = "nombre")]
        public string Name { get; set; }
    }

}
