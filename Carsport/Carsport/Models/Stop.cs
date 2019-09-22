namespace Carsport.Models
{
    using Newtonsoft.Json;

    public class Stop
    {
        [JsonProperty(PropertyName = "idParada")]
        public string StopId { get; set; }

        [JsonProperty(PropertyName = "idLinea")]
        public string LineId { get; set; }

        [JsonProperty(PropertyName = "idNucleo")]
        public string CoreId { get; set; }

        [JsonProperty(PropertyName = "idZona")]
        public string ZoneId { get; set; }

        [JsonProperty(PropertyName = "nombre")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "latitud")]
        public string Latitude { get; set; }

        [JsonProperty(PropertyName = "longitud")]
        public string Longitude { get; set; }

        [JsonProperty(PropertyName = "sentido")]
        public string Sense { get; set; }

        [JsonProperty(PropertyName = "orden")]
        public int Order { get; set; }

        [JsonProperty(PropertyName = "modos")]
        public string Modes { get; set; }
    }
}
