namespace Carsport.Models
{
    using Newtonsoft.Json;

    public class Commune
    {
        [JsonProperty(PropertyName = "idMunicipio")]
        public string CommuneId { get; set; }

        [JsonProperty(PropertyName = "datos")]
        public string Name { get; set; }
    }
}
