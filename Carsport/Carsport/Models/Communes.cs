namespace Carsport.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class Communes
    {
        [JsonProperty(PropertyName = "municipios")]
        public List<Commune> CommuneList { get; set; }
    }
}
