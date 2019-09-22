namespace Carsport.Models
{
    using Newtonsoft.Json;

    public class Town
    {
        [JsonProperty(PropertyName = "colspan")]
        public int Colspan { get; set; }

        [JsonProperty(PropertyName = "nombre")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "color")]
        public string Colour { get; set; }
    }

}
