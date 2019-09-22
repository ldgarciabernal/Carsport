namespace Carsport.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Timetable
    {
        [JsonProperty(PropertyName = "bloques")]
        public List<Block> Bloks { get; set; }

        [JsonProperty(PropertyName = "horario")]
        public List<Shedule> Shedules { get; set; }

        [JsonProperty(PropertyName = "frecuencias")]
        public List<Frequency> Frequencies { get; set; }

        [JsonProperty(PropertyName = "nucleos")]
        public List<Town> Towns { get; set; }
    }
}
