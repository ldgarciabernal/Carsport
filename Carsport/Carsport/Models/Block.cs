namespace Carsport.Models
{
    using Newtonsoft.Json;

    public class Block
    {
        [JsonProperty(PropertyName = "nombre")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "color")]
        public string Colour { get; set; }

        [JsonProperty(PropertyName = "tipo")]
        public string Type { get; set; }

    }
}
