namespace Carsport.Models
{
    using Newtonsoft.Json;

    public class Line
    {
        [JsonProperty(PropertyName = "idLinea")]
        public string LineId { get; set; }

        [JsonProperty(PropertyName = "codigo")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "nombre")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "hayNoticias")]
        public string Notice { get; set; }

        [JsonProperty(PropertyName = "modo")]
        public string Mode { get; set; }

        [JsonProperty(PropertyName = "idModo")]
        public string ModeId { get; set; }

        [JsonProperty(PropertyName = "operadores")]
        public string Operator { get; set; }

        public override string ToString()
        {
            return this.Code + " - " + this.Name;
        }
    }
}
