namespace Carsport.Models
{
    using Newtonsoft.Json;

    public class Core
    {
        [JsonProperty(PropertyName = "idNucleo")]
        public string CoreId { get; set; }

        [JsonProperty(PropertyName = "idMunicipio")]
        public string CommuneId { get; set; }

        [JsonProperty(PropertyName = "idZona")]
        public string ZoneId { get; set; }

        [JsonProperty(PropertyName = "nombre")]
        public string Name { get; set; }
    }

}
